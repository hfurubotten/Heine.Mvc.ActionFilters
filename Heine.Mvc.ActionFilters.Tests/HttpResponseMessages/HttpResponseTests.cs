using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Heine.Mvc.ActionFilters.Extensions;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.HttpResponseMessages
{
    public class HttpResponseTests
    {
        [Test]
        public void TestThatStreamIsRemovedFromByteRangeResponse()
        {
            // Arrange
            var rArray = Encoding.ASCII.GetBytes("ASAWERKFEOFdarqfaowpJAWHEAW#¤!!¤&");
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
        
        [Test]
        public void TestThatStreamIsRemovedFromStreamContentResponse()
        {
            // Arrange
            var rArray = Encoding.ASCII.GetBytes("ASAWERKFEOFdarqfaowpJAWHEAW#¤!!¤&");
            var mediaType = new MediaTypeHeaderValue("application/pdf");
            var stream = new MemoryStream(rArray);

            var response = new HttpResponseMessage
            {
                RequestMessage = new HttpRequestMessage(),
                
                Content = new StreamContent(stream)
                {
                    Headers = {ContentType = mediaType}
                }
            };

            // Act
            response.Destruct();
            var checkVariable = response.Content.ReadAsString(response.Headers);

            // Assert
            Assert.AreEqual(string.Empty, checkVariable);
        }
    }
}
