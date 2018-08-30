using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var rArray = new byte[50];
            var mediaType = new MediaTypeHeaderValue("application/pdf");

            var mockHttpRequestMessage = new HttpRequestMessage
            {
                Content = new ByteRangeStreamContent(new MemoryStream(rArray), new RangeHeaderValue(0, 0), mediaType)
            };

            // Act
            mockHttpRequestMessage.Destruct();
            var checkVariable = mockHttpRequestMessage.Content.ReadAsString(mockHttpRequestMessage.Headers);

            // Assert
            Assert.AreEqual(string.Empty, checkVariable);
        }
    }
}
