using System;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HttpContentExtensions
    {
        public static string ReadAsString(this HttpContent httpContent)
        {
            if (httpContent == null) return string.Empty;

            string content;

            try
            {
                content = httpContent.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (NotSupportedException e)
            {
                content = $"Unable to read body of HTTP content:\n{string.Join("\n", e.GetMessages().Select(message => $"- '{message}'"))}";
            }

            return content;
        }

        public static object ReadAsObject(this HttpContent httpContent)
        {
            if (httpContent == null) return null;

            var body = httpContent.ReadAsString();

            switch (httpContent.Headers?.ContentType?.MediaType)
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