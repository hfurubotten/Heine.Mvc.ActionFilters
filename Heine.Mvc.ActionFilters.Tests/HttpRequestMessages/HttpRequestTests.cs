using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Heine.Mvc.ActionFilters.Extensions;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.HttpRequestMessages
{
    class HttpRequestTests
    {
        [Test]
        public void TestThatStreamIsRemovedFromRequest()
        {
            // Arrange
            var randomBytesArray = Encoding.ASCII.GetBytes("ASAWERKFEOFdarqfaowpJAWHEAW#¤!!¤&");
            var mediaType = new MediaTypeHeaderValue("application/pdf");

            var mockHttpRequestMessage = new HttpRequestMessage
            {
                Content = new ByteRangeStreamContent(new MemoryStream(randomBytesArray), new RangeHeaderValue(0, 0), mediaType)
            };

            // Act
            mockHttpRequestMessage.Destruct();
            var checkVariable = mockHttpRequestMessage.Content.ReadAsString(mockHttpRequestMessage.Headers);

            // Assert
            Assert.AreEqual(string.Empty, checkVariable);
        }

        [Test]
        public void TestThatStreamIsRemovedFromStreamContentRequest()
        {
            // Arrange
            var rArray = Encoding.ASCII.GetBytes("ASAWERKFEOFdarqfaowpJAWHEAW#¤!!¤&");
            var mediaType = new MediaTypeHeaderValue("application/pdf");
            var stream = new MemoryStream(rArray);

            var mockHttpRequestMessage = new HttpRequestMessage
            {
                Content = new StreamContent(stream)
                {
                    Headers = { ContentType = mediaType }
                }
            };

            // Act
            mockHttpRequestMessage.Destruct();
            var checkVariable = mockHttpRequestMessage.Content.ReadAsString(mockHttpRequestMessage.Headers);

            // Assert
            Assert.AreEqual(string.Empty, checkVariable);
        }
    }
}
