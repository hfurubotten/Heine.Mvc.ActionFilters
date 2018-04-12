using System;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HttpContentExtensions
    {
        public static HttpContent Clone(this HttpContent content)
        {
            if (content == null) return null;
            
            var stream = content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            stream.Position = 0;

            var clone = new StreamContent(stream);

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
                content = httpContent.Clone().ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (NotSupportedException e)
            {
                content = $"Unable to read body of HTTP content:\n{string.Join("\n", e.GetMessages().Select(message => $"- '{message}'"))}";
            }

            return content;
        }

        public static object ReadAsObject(this HttpContent httpContent)
        {
            var body = httpContent?.ReadAsString();

            switch (httpContent?.Headers?.ContentType?.MediaType)
            {
                case "application/json":
                    try { return JsonConvert.DeserializeObject(body); }
                    catch { return body; }
                case "application/xml":
                    try { return XDocument.Parse(body); }
                    catch { return body; }
            }

            return body;
        }
    }
}