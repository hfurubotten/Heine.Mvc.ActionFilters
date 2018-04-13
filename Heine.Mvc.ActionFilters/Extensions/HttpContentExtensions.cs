using System;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HttpContentExtensions
    {
        public static string ReadAsString(this HttpContent httpContent, bool format = true)
        {
            if (httpContent == null) return string.Empty;

            string content;

            try
            {
                content = httpContent.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                if (format)
                {
                    switch (httpContent.Headers?.ContentType?.MediaType)
                    {
                        case "application/json":
                            try { return JToken.Parse(content).ToString(Formatting.Indented); }
                            catch { return content; }
                        case "application/xml":
                            try { return XDocument.Parse(content).ToString(); }
                            catch { return content; }
                    }
                }
            }
            catch (Exception ex)
            {
                content = $"Unable to read body of HTTP content:\n{string.Join("\n", ex.GetMessages().Select(message => $"- '{message}'"))}";
            }

            return content;
        }
    }
}