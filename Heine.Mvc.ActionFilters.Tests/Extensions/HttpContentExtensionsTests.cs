using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using FluentAssertions;
using Heine.Mvc.ActionFilters.Extensions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.Extensions
{
    [TestFixture]
    public class HttpContentExtensionsTests
    {
        private HttpResponseMessage httpResponseMessage;

        [SetUp]
        public void Init()
        {
            httpResponseMessage = new HttpResponseMessage();
        }

        [TestCase("Document", PascalCaseContent)]
        [TestCase("document", CamelCaseContent)]
        public void Obfuscate_Content_CaseInsensitive(string obfuscateValue, string jsonContent)
        {
            // Arrange
            httpResponseMessage.Headers.TryAddWithoutValidation("X-Obfuscate", new List<string> {"Document"});
            httpResponseMessage.Content = new StringContent(jsonContent);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers, true);
            var obj = JObject.Parse(content);

            // Assert
            obj.Should().NotBeNull();
            obj[obfuscateValue].Value<string>().Should().Be(ObfuscateValue);
        }

        [TestCase("Document", PascalCaseContentArray)]
        [TestCase("document", CamelCaseContentArray)]
        public void Obfuscate_Content_CaseInsensitive_WhenContentRootIsArray(string obfuscateValue, string jsonContent)
        {
            // Arrange
            httpResponseMessage.Headers.TryAddWithoutValidation("X-Obfuscate", new List<string> {"Document"});
            httpResponseMessage.Content = new StringContent(jsonContent);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers, true);
            var arr = JArray.Parse(content);

            // Assert
            arr.Should().NotBeNull();
            arr.First[obfuscateValue].Value<string>().Should().Be(ObfuscateValue);
        }

        [TestCase("ExternalRecipients", "EmailAddress", PascalCaseContent)]
        [TestCase("externalRecipients", "emailAddress", CamelCaseContent)]
        public void Obfuscate_PropertyInArray_CaseInsensitive(string obfuscateArray, string obfuscateProperty,
            string jsonContent)
        {
            // Arrange
            httpResponseMessage.Headers.TryAddWithoutValidation("X-Obfuscate",
                new List<string> {"ExternalRecipients[*].EmailAddress"});
            httpResponseMessage.Content = new StringContent(jsonContent);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers, true);
            var obj = JObject.Parse(content);

            // Assert
            obj.Should().NotBeNull();
            obj[obfuscateArray].Should().NotBeNull();
            foreach (var item in obj[obfuscateArray])
            {
                item[obfuscateProperty].Value<string>().Should().Be(ObfuscateValue);
            }
        }

        [TestCase("ExternalRecipients", "EmailAddress", PascalCaseContentArray)]
        [TestCase("externalRecipients", "emailAddress", CamelCaseContentArray)]
        public void Obfuscate_PropertyInArray_CaseInsensitive_WhenContentRootIsArray(string obfuscateArray,
            string obfuscateProperty, string jsonContent)
        {
            // Arrange
            httpResponseMessage.Headers.TryAddWithoutValidation("X-Obfuscate",
                new List<string> {"ExternalRecipients[*].EmailAddress"});
            httpResponseMessage.Content = new StringContent(jsonContent);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers, true);
            var arr = JArray.Parse(content);

            // Assert
            arr.Should().NotBeNull();
            arr.First[obfuscateArray].Should().NotBeNull();
            foreach (var item in arr.First[obfuscateArray])
            {
                item[obfuscateProperty].Value<string>().Should().Be(ObfuscateValue);
            }
        }

        [TestCase("children", "content", ODataValueContent)]
        public void Obfuscate_PropertyInArray_OData(string obfuscateArray, string obfuscateProperty, string jsonContent)
        {
            // Arrange
            httpResponseMessage.Headers.TryAddWithoutValidation("X-Obfuscate",
                new List<string> {"Children[*].Content"});
            httpResponseMessage.Content = new StringContent(jsonContent);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers, true);
            var obj = JObject.Parse(content);

            // Assert
            obj.Should().NotBeNull();
            obj["value"].Should().NotBeNull();
            foreach (var items in obj["value"])
            {
                foreach (var item in items[obfuscateArray])
                    item[obfuscateProperty].Value<string>().Should().Be(ObfuscateValue);
            }
        }

        [TestCase("Document", "Private", "ExternalRecipients", PascalCaseContent)]
        [TestCase("document", "private", "externalRecipients", CamelCaseContent)]
        public void Obfuscate_PropertyObjectArray_CaseInsensitive(string obfuscateProperty, string obfuscateObject,
            string obfuscateArray, string jsonContent)
        {
            // Arrange
            httpResponseMessage.Headers.TryAddWithoutValidation("X-Obfuscate",
                new List<string> {"Document", "Private", "ExternalRecipients"});
            httpResponseMessage.Content = new StringContent(jsonContent);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers, true);
            var obj = JObject.Parse(content);

            // Assert
            obj.Should().NotBeNull();
            obj[obfuscateProperty].Value<string>().Should().Be(ObfuscateValue);

            foreach (var property in obj[obfuscateObject])
            {
                property.First.Value<string>().Should().Be(ObfuscateValue);
            }

            foreach (var element in obj[obfuscateArray])
            {
                foreach (var property in element)
                {
                    property.First.Value<string>().Should().Be(ObfuscateValue);
                }
            }
        }

        [TestCase("Document", "Private", "ExternalRecipients", PascalCaseContentArray)]
        [TestCase("document", "private", "externalRecipients", CamelCaseContentArray)]
        public void Obfuscate_PropertyObjectArray_CaseInsensitive_WhenContentRootIsArray(string obfuscateProperty,
            string obfuscateObject, string obfuscateArray, string jsonContent)
        {
            // Arrange
            httpResponseMessage.Headers.TryAddWithoutValidation("X-Obfuscate",
                new List<string> {"Document", "Private", "ExternalRecipients"});
            httpResponseMessage.Content = new StringContent(jsonContent);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers, true);
            var arr = JArray.Parse(content);
            var obj = arr.First;

            // Assert
            obj.Should().NotBeNull();
            obj[obfuscateProperty].Value<string>().Should().Be(ObfuscateValue);

            foreach (var property in obj[obfuscateObject])
            {
                property.First.Value<string>().Should().Be(ObfuscateValue);
            }

            foreach (var element in obj[obfuscateArray])
            {
                foreach (var property in element)
                {
                    property.First.Value<string>().Should().Be(ObfuscateValue);
                }
            }
        }

        [TestCase("format", "archiveReference", "children", ODataValueContent)]
        public void Obfuscate_PropertyObjectArray_OData(string obfuscateProperty,
            string obfuscateObject, string obfuscateArray, string jsonContent)
        {
            // Arrange
            httpResponseMessage.Headers.TryAddWithoutValidation("X-Obfuscate",
                new List<string> { "Format", "ArchiveReference", "Children" });
            httpResponseMessage.Content = new StringContent(jsonContent);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers, true);
            var obj = JObject.Parse(content);

            // Assert
            obj.Should().NotBeNull();
            obj["value"].Should().NotBeNull();
            foreach (var item in obj["value"])
            {
                item[obfuscateProperty].Value<string>().Should().Be(ObfuscateValue);

                foreach (var property in item[obfuscateObject])
                    property.First.Value<string>().Should().Be(ObfuscateValue);

                foreach (var element in item[obfuscateArray])
                {
                    foreach (var property in element)
                    {
                        property.First.Value<string>().Should().Be(ObfuscateValue);
                    }
                }
            }
        }

        private const string PascalCaseContent =
            "{\"CaseNumber\":\"2017/901674\",\"CaseHandler\":\"hefur\",\"Document\":\"0AE132314QFQESA\",\"Title\":\"IntegrationTestDocument\",\"JournalTitle\":\"IntegrationTestDocument\",\"Format\":\"pdf\",\"AccountName\":\"TestAccount\",\"AccountId\":\"923313850\",\"Type\":\"Outgoing\",\"Private\":{\"Name\":\"PetterPettersen\",\"AccountNo\":\"123456789\"},\"ExternalRecipients\":[{\"Account\":{\"Name\":\"PålFredriksen\",\"AccountNo\":\"987654321\"},\"Relationships\":[{\"Name\":\"Hansen\"},{\"Tlf\":\"92247789\"}],\"AccountId\":\"923313850\",\"IsCopyRecipient\":false,\"FullName\":\"PålFredriksen\",\"EmailAddress\":\"test1@test.net\",\"Mobile\":\"92247881\"},{\"Account\":{\"Name\":\"PålHansen\",\"AccountNo\":\"852741369\"},\"Relationships\":[{\"Name\":\"Hansen\"},{\"Tlf\":\"92247789\"}],\"AccountId\":\"923313851\",\"IsCopyRecipient\":false,\"FullName\":\"PålHansen\",\"EmailAddress\":\"test2@test.net\",\"Mobile\":\"92247882\"}],\"InternalRecipients\":[{\"Username\":\"hefur\",\"IsCopyRecipient\":false,\"ExemptFromPublic\":true}],\"AccessCode\":\"UO\",\"JournalStatus\":\"F\"}";

        private const string CamelCaseContent =
            "{\"caseNumber\":\"2017/901674\",\"caseHandler\":\"hefur\",\"document\":\"0AE132314QFQESA\",\"title\":\"Integration Test Document\",\"journalTitle\":\"Integration Test Document\",\"format\":\"pdf\",\"accountName\":\"Test Account\",\"accountId\":\"923313850\",\"type\":\"Outgoing\",\"private\":{\"name\":\"Petter Pettersen\",\"accountNo\":\"123456789\"},\"externalRecipients\":[{\"account \":{\"name\":\"Pål Fredriksen\",\"accountNo\":\"987654321\"},\"relationships\":[{\"name\":\"Hansen\"},{\"tlf\":\"92247789\"}],\"accountId\":\"923313850\",\"isCopyRecipient\":false,\"fullName\":\"Pål Fredriksen\",\"emailAddress\":\"test1@test.net\",\"mobile\":\"92247881\"},{\"account \":{\"name\":\"Pål Hansen\",\"accountNo\":\"852741369\"},\"relationships\":[{\"name\":\"Hansen\"},{\"tlf\":\"92247789\"}],\"accountId\":\"923313851\",\"isCopyRecipient\":false,\"fullName\":\"Pål Hansen\",\"emailAddress\":\"test2@test.net\",\"mobile\":\"92247882\"}],\"internalRecipients\":[{\"username\":\"hefur\",\"isCopyRecipient\":false,\"exemptFromPublic\":true}],\"accessCode\":\"UO\",\"journalStatus\":\"F\"}";

        private const string PascalCaseContentArray =
            "[{\"CaseNumber\":\"2017/901674\",\"CaseHandler\":\"hefur\",\"Document\":\"0AE132314QFQESA\",\"Title\":\"IntegrationTestDocument\",\"JournalTitle\":\"IntegrationTestDocument\",\"Format\":\"pdf\",\"AccountName\":\"TestAccount\",\"AccountId\":\"923313850\",\"Type\":\"Outgoing\",\"Private\":{\"Name\":\"PetterPettersen\",\"AccountNo\":\"123456789\"},\"ExternalRecipients\":[{\"Account\":{\"Name\":\"PålFredriksen\",\"AccountNo\":\"987654321\"},\"Relationships\":[{\"Name\":\"Hansen\"},{\"Tlf\":\"92247789\"}],\"AccountId\":\"923313850\",\"IsCopyRecipient\":false,\"FullName\":\"PålFredriksen\",\"EmailAddress\":\"test1@test.net\",\"Mobile\":\"92247881\"},{\"Account\":{\"Name\":\"PålHansen\",\"AccountNo\":\"852741369\"},\"Relationships\":[{\"Name\":\"Hansen\"},{\"Tlf\":\"92247789\"}],\"AccountId\":\"923313851\",\"IsCopyRecipient\":false,\"FullName\":\"PålHansen\",\"EmailAddress\":\"test2@test.net\",\"Mobile\":\"92247882\"}],\"InternalRecipients\":[{\"Username\":\"hefur\",\"IsCopyRecipient\":false,\"ExemptFromPublic\":true}],\"AccessCode\":\"UO\",\"JournalStatus\":\"F\"}]";

        private const string CamelCaseContentArray =
            "[{\"caseNumber\":\"2017/901674\",\"caseHandler\":\"hefur\",\"document\":\"0AE132314QFQESA\",\"title\":\"Integration Test Document\",\"journalTitle\":\"Integration Test Document\",\"format\":\"pdf\",\"accountName\":\"Test Account\",\"accountId\":\"923313850\",\"type\":\"Outgoing\",\"private\":{\"name\":\"Petter Pettersen\",\"accountNo\":\"123456789\"},\"externalRecipients\":[{\"account \":{\"name\":\"Pål Fredriksen\",\"accountNo\":\"987654321\"},\"relationships\":[{\"name\":\"Hansen\"},{\"tlf\":\"92247789\"}],\"accountId\":\"923313850\",\"isCopyRecipient\":false,\"fullName\":\"Pål Fredriksen\",\"emailAddress\":\"test1@test.net\",\"mobile\":\"92247881\"},{\"account \":{\"name\":\"Pål Hansen\",\"accountNo\":\"852741369\"},\"relationships\":[{\"name\":\"Hansen\"},{\"tlf\":\"92247789\"}],\"accountId\":\"923313851\",\"isCopyRecipient\":false,\"fullName\":\"Pål Hansen\",\"emailAddress\":\"test2@test.net\",\"mobile\":\"92247882\"}],\"internalRecipients\":[{\"username\":\"hefur\",\"isCopyRecipient\":false,\"exemptFromPublic\":true}],\"accessCode\":\"UO\",\"journalStatus\":\"F\"}]";

        private const string ODataValueContent =
            "{\"@odata.context\":\"some context\",\"value\":[{\"title\":\"Kredittsjekk - 15.05.2019\",\"format\":\"PDF\",\"archiveReference\":{\"id\":\"959160ff-3563-462a-96de-88b0713bf248\",\"archiveDocumentId\":1495448,\"journalId\":824435,\"caseId\":158036,\"name\":\"Kredittsjekk - 15.05.2019\",\"version\":1,\"archivedOn\":\"2019-05-15T08:35:45.0697052Z\",\"documentDirection\":\"Internal\"},\"children\":[]},{\"title\":\"Søknad om tradisjonell bruksutbygging\",\"format\":\"HTML\",\"archiveReference\":{\"id\":\"f453a994-9c4a-4098-b482-d14b5651b497\",\"archiveDocumentId\":1495446,\"journalId\":824434,\"caseId\":158036,\"name\":\"Søknad om tradisjonell bruksutbygging\",\"version\":1,\"archivedOn\":\"2019-05-15T08:35:22.1412998Z\",\"documentDirection\":\"Incoming\"},\"children\":[{\"id\":\"cf0862c9-8285-4756-9f9d-2fb8122e1f83\",\"ownerId\":\"0e85a952-8333-4dd0-a6cc-e5f67fa8684b\",\"regardingId\":\"0e85a952-8333-4dd0-a6cc-e5f67fa8684b\",\"regardingEntity\":\"ci_application\",\"format\":\"pdf\",\"title\":\"GPL License Terms.pdf\",\"content\":\"JVBERi0xLjQNJeLjz9MNCjE\",\"type\":\"Attachment\",\"parentId\":\"f453a994-9c4a-4098-b482-d14b5651b497\",\"revision\":0,\"revisionOfId\":null,\"createdOn\":\"2019-05-15T08:34:27.2572211Z\",\"modifiedOn\":\"2019-05-15T08:34:27.2572211Z\",\"createdBy\":\"59ece3d7-2478-441a-98b6-bba58adf9304\",\"modifiedBy\":\"59ece3d7-2478-441a-98b6-bba58adf9304\",\"archiveReference\":{\"id\":\"cf0862c9-8285-4756-9f9d-2fb8122e1f83\",\"archiveDocumentId\":1495447,\"journalId\":824434,\"caseId\":0,\"name\":\"GPL License Terms.pdf\",\"version\":0,\"archivedOn\":\"2019-05-15T10:35:23.173925Z\",\"documentDirection\":\"Incoming\"},\"relationships\":[{\"name\":\"Petter Pettersen\",\"accountNo\":\"12345678\"},{\"name\":\"Petter Tork\",\"accountNo\":\"9876542\"}]}]}]}";

        private static string ObfuscateValue => "*** OBFUSCATED ***";
    }
}