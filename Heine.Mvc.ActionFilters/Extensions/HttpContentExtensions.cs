using System.IO;
using System.Net.Http;
using System.Text;
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

        public static string AsFormattedString(this HttpContent content)
        {
            string ReadContent()
            {
                var stringContent = content.GetBody();

                switch (content.Headers?.ContentType?.MediaType)
                {
                    case "application/json":
                        stringContent = JToken.Parse(stringContent).ToString(Formatting.Indented).Replace(@"\r\n", "\n");
                        break;
                    case "application/xml":
                        //TODO: Prettify XML etc.
                        //content = XmlConvert.Prettify(content);
                        break;
                }

                return stringContent;
            }

            if (content == null) return string.Empty;

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Body:");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine(ReadContent());
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }
    }
}