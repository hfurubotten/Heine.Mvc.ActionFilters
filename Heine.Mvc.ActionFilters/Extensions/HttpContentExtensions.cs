using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HttpContentExtensions
    {
        internal static string GetBody(this HttpContent content)
        {
            if (content == null) return null;

            var requestStream = content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            requestStream.Position = 0;
            var streamReader = new StreamReader(requestStream, Encoding.UTF8);
            var requestBody = streamReader.ReadToEnd();
            requestStream.Position = 0;
            return requestBody;
        }

        public static string AsFormattedString(this HttpContent httpContent)
        {
            string ReadContent()
            {
                var body = httpContent.GetBody();

                switch (httpContent.Headers?.ContentType?.MediaType)
                {
                    case "application/json":
                        try { body = JToken.Parse(body).ToString(Formatting.Indented).Replace(@"\r\n", "\n"); }
                        catch { return body; }
                        break;
                    case "application/xml":
                        try { body = XDocument.Parse(body).ToString(); }
                        catch { return body; }
                        break;
                }

                return body;
            }

            if (httpContent == null) return string.Empty;

            var stringBuilder = new StringBuilder();

            var content = ReadContent();

            if (!string.IsNullOrWhiteSpace(content))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Body:");
                stringBuilder.AppendLine(content);
            }

            return stringBuilder.ToString();
        }
    }
}