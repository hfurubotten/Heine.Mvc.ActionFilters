using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Heine.Mvc.ActionFilters.Extensions;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.HttpResponseMessages
{
    public class HttpResponseTests
    {
        [Test]
        public void TestThatStreamIsRemovedFromResponse()
        {
            // Arrange
            var rArray = new byte[50];
            var mediaType = new MediaTypeHeaderValue("application/pdf");

            var mockHttpRequestMessage = new HttpRequestMessage
            {
                Content = new ByteRangeStreamContent(new MemoryStream(rArray), new RangeHeaderValue(0, 0), mediaType)
            };

            var httpResponse = mockHttpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponse.Content = new ByteRangeStreamContent(new MemoryStream(rArray), new RangeHeaderValue(0,0),mediaType);

            // Act
            httpResponse.Destruct();
            var checkVariable = httpResponse.Content.ReadAsString(mockHttpRequestMessage.Headers);

            // Assert
            Assert.AreEqual(string.Empty, checkVariable);
        }
    }
}
