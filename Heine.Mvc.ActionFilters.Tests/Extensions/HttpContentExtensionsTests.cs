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
            httpResponseMessage.Headers.TryAddWithoutValidation("X-Obfuscate", ObfuscateProperties);
        }

        [Test]
        public void Obfuscate_PascalCaseContent()
        {
            // Arrange
            httpResponseMessage.Content = new StringContent(PascalCaseContent);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers);
            var obj = JObject.Parse(content);

            // Assert
            obj.Should().NotBeNull();
            obj["Document"].Value<string>().Should().Be(ObfuscateValue);
        }

        [Test]
        public void Obfuscate_CamelCaseContent()
        {
            // Arrange
            httpResponseMessage.Content = new StringContent(CamelCaseContent);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers);
            var obj = JObject.Parse(content);

            // Assert
            obj.Should().NotBeNull();
            obj["document"].Value<string>().Should().Be(ObfuscateValue);
        }

        [Test]
        public void Obfuscate_PascalCaseContent_WhenContentIsInArray()
        {
            // Arrange
            httpResponseMessage.Content = new StringContent(PascalCaseContentArray);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers);
            var arr = JArray.Parse(content);

            // Assert
            arr.Should().NotBeNull();
            arr.First["Document"].Value<string>().Should().Be(ObfuscateValue);
        }

        [Test]
        public void Obfuscate_CamelCaseContent_WhenContentIsInInArray()
        {
            // Arrange
            httpResponseMessage.Content = new StringContent(CamelCaseContentArray);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var content = httpResponseMessage.Content.ReadAsString(httpResponseMessage.Headers);
            var arr = JArray.Parse(content);

            // Assert
            arr.Should().NotBeNull();
            arr.First["document"].Value<string>().Should().Be(ObfuscateValue);
        }

        private static string PascalCaseContent => "{\"CaseNumber\":\"2017/901674\",\"CaseHandler\":\"hefur\",\"Document\":\"0AE132314QFQESA\",\"Title\":\"Integration Test Document\",\"JournalTitle\":\"Integration Test Document\",\"Format\":\"pdf\",\"AccountName\":\"Test Account\",\"AccountId\":\"923313850\",\"Type\":\"Outgoing\",\"ExternalRecipients\":[{\"AccountName\":\"Test Recipient\",\"AccountId\":\"923313850\",\"IsCopyRecipient\":false,\"FullName\":\"Test User\",\"EmailAddress\":\"test@test.net\",\"Mobile\":\"92247881\"},{\"AccountName\":\"Test Recipient 2\",\"AccountId\":\"923313851\",\"IsCopyRecipient\":false,\"FullName\":\"Test User 2\",\"EmailAddress\":\"test@test.net\",\"Mobile\":\"92247882\"}],\"InternalRecipients\":[{\"Username\":\"hefur\",\"IsCopyRecipient\":false,\"ExemptFromPublic\":true}],\"AccessCode\":\"UO\",\"JournalStatus\":\"F\"}";

        private static string CamelCaseContent => "{\"caseNumber\":\"2017/901674\",\"caseHandler\":\"hefur\",\"document\":\"0AE132314QFQESA\",\"title\":\"Integration Test Document\",\"journalTitle\":\"Integration Test Document\",\"format\":\"pdf\",\"accountName\":\"Test Account\",\"accountId\":\"923313850\",\"type\":\"Outgoing\",\"externalRecipients\":[{\"accountName\":\"Test Recipient\",\"accountId\":\"923313850\",\"isCopyRecipient\":false,\"fullName\":\"Test User\",\"emailAddress\":\"test@test.net\",\"mobile\":\"92247881\"},{\"accountName\":\"Test Recipient 2\",\"accountId\":\"923313851\",\"isCopyRecipient\":false,\"fullName\":\"Test User 2\",\"emailAddress\":\"test@test.net\",\"mobile\":\"92247882\"}],\"internalRecipients\":[{\"username\":\"hefur\",\"isCopyRecipient\":false,\"exemptFromPublic\":true}],\"accessCode\":\"UO\",\"journalStatus\":\"F\"}";

        private static string PascalCaseContentArray => "[{\"CaseNumber\":\"2017/901674\",\"CaseHandler\":\"hefur\",\"Document\":\"0AE132314QFQESA\",\"Title\":\"Integration Test Document\",\"JournalTitle\":\"Integration Test Document\",\"Format\":\"pdf\",\"AccountName\":\"Test Account\",\"AccountId\":\"923313850\",\"Type\":\"Outgoing\",\"ExternalRecipients\":[{\"AccountName\":\"Test Recipient\",\"AccountId\":\"923313850\",\"IsCopyRecipient\":false,\"FullName\":\"Test User\",\"EmailAddress\":\"test@test.net\",\"Mobile\":\"92247881\"},{\"AccountName\":\"Test Recipient 2\",\"AccountId\":\"923313851\",\"IsCopyRecipient\":false,\"FullName\":\"Test User 2\",\"EmailAddress\":\"test@test.net\",\"Mobile\":\"92247882\"}],\"InternalRecipients\":[{\"Username\":\"hefur\",\"IsCopyRecipient\":false,\"ExemptFromPublic\":true}],\"AccessCode\":\"UO\",\"JournalStatus\":\"F\"}]";

        private static string CamelCaseContentArray => "[{\"caseNumber\":\"2017/901674\",\"caseHandler\":\"hefur\",\"document\":\"0AE132314QFQESA\",\"title\":\"Integration Test Document\",\"journalTitle\":\"Integration Test Document\",\"format\":\"pdf\",\"accountName\":\"Test Account\",\"accountId\":\"923313850\",\"type\":\"Outgoing\",\"externalRecipients\":[{\"accountName\":\"Test Recipient\",\"accountId\":\"923313850\",\"isCopyRecipient\":false,\"fullName\":\"Test User\",\"emailAddress\":\"test@test.net\",\"mobile\":\"92247881\"},{\"accountName\":\"Test Recipient 2\",\"accountId\":\"923313851\",\"isCopyRecipient\":false,\"fullName\":\"Test User 2\",\"emailAddress\":\"test@test.net\",\"mobile\":\"92247882\"}],\"internalRecipients\":[{\"username\":\"hefur\",\"isCopyRecipient\":false,\"exemptFromPublic\":true}],\"accessCode\":\"UO\",\"journalStatus\":\"F\"}]";

        private static string ObfuscateValue => "*** OBFUSCATED ***";

        private static string ObfuscateProperties => "Document";
    }
}