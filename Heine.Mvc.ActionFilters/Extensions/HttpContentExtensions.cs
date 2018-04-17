using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HttpContentExtensions
    {
        public static string ReadAsString(this HttpContent httpContent, HttpHeaders httpHeaders)
        {
            try
            {
                if (httpContent == null) return string.Empty;

                var stream = httpContent.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                stream.Position = 0;

                // The stream returned by `ReadAsStreamAsync()` is the same stream used by `ReadAsStringAsync()`, 
                // requiring us to reset the stream position before reading it in case it has already been consumed.

                var content = httpContent.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                // Resetting the stream again, just in case other consumers of the stream doesn't reset the stream position before trying read it.
                stream.Position = 0;

                return Format(content);
            }
            catch (Exception ex)
            {
                return $"Unable to read body of HTTP content:\n{string.Join("\n", ex.GetMessages().Select(message => $"- '{message}'"))}";
            }

            string Format(string content)
            {
                switch (httpContent.Headers?.ContentType?.MediaType)
                {
                    case "application/json":
                        try { return Obfuscate(JObject.Parse(content), httpHeaders).ToString(Formatting.Indented); }
                        catch { return content; }
                    case "application/xml":
                        try { return XDocument.Parse(content).ToString(); }
                        catch { return content; }
                    default:
                        return content;
                }
            }

            JObject Obfuscate(JObject jObject, HttpHeaders headers)
            {
                if (headers.TryGetValues("X-Obfuscate", out var values))
                {
                    var properties = jObject.Children<JProperty>().ToDictionary(k => k.Name);
                    foreach (var value in values)
                    {
                        if (properties.TryGetValue(value, out var jProperty))
                        {
                            jProperty.Value = "*** OBFUSCATED ***";
                        }
                    }
                }
                return jObject;
            }
        }
    }
}