using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HttpContentExtensions
    {
        public static HttpContent Clone(this HttpContent content)
        {
            if (content == null) return null;

            var ms = new MemoryStream();
            content.CopyToAsync(ms).Wait();
            ms.Position = 0;

            var clone = new StreamContent(ms);
            foreach (var header in content.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }
            return clone;
        }

        public static string ReadAsString(this HttpContent httpContent)
        {
            if (httpContent == null) return string.Empty;

            string content;

            try
            {
                var contentStream = httpContent.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                contentStream.Position = 0;
                var streamReader = new StreamReader(contentStream, Encoding.UTF8);
                content = streamReader.ReadToEnd();
                contentStream.Position = 0;
            }
            catch (NotSupportedException e)
            {
                content = $"Unable to read body of HTTP content:\n{string.Join("\n", e.GetMessages().Select(message => $"- '{message}'"))}";
            }

            return content;
        }

        public static string AsFormattedString(this HttpContent httpContent)
        {
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

            string ReadContent()
            {
                var body = httpContent.ReadAsString();

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
        }

        internal static object ReadContent(this HttpContent httpContent)
        {
            var body = httpContent?.ReadAsString();

            switch (httpContent?.Headers?.ContentType?.MediaType)
            {
                case "application/json":
                    try { return JToken.Parse(body); }
                    catch { return body; }
                case "application/xml":
                    try { return XDocument.Parse(body); }
                    catch { return body; }
            }

            return body;
        }
    }
}