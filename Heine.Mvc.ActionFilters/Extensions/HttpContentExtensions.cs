using System.IO;
using System.Net.Http;
using System.Text;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HttpContentExtensions
    {
        internal static string GetBody(this HttpContent content)
        {
            if (content == null) return null;

            var requestStream = content.ReadAsStreamAsync().Result;
            requestStream.Position = 0;
            var streamReader = new StreamReader(requestStream, Encoding.UTF8);
            var requestBody = streamReader.ReadToEnd();
            return requestBody;
        }
    }
}
