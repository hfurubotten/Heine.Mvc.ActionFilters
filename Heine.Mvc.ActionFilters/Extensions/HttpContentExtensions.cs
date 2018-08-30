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
                if (httpContent == null || httpContent is StreamContent || httpContent is ByteRangeStreamContent ) return string.Empty;
                
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
                        try { return Obfuscate(JToken.Parse(content), httpHeaders).ToString(Formatting.Indented); }
                        catch { return content; }
                    case "application/xml":
                        try { return XDocument.Parse(content).ToString(); }
                        catch { return content; }
                    case "application/pdf":
                        return string.Empty;
                    default:
                        return content;
                }
            }

            JToken Obfuscate(JToken jToken, HttpHeaders headers)
            {
                if (headers.TryGetValues("X-Obfuscate", out var properties))
                {
                    var jPath = jToken is JArray ? (IsArray: true, Path: "$[*]") : (IsArray: false, Path: "$");
                    foreach (var property in properties)
                    {
                        if (jPath.IsArray)
                        {
                            foreach (var item in jToken.SelectTokens($"{jPath.Path}.{property}"))
                            {
                                if (!item.IsNullOrEmpty())
                                    item.Replace("*** OBFUSCATED ***");
                            }
                        }
                        else
                        {
                            var token = jToken.SelectToken($"{jPath.Path}.{property}");
                            if (!token.IsNullOrEmpty())
                                token.Replace("*** OBFUSCATED ***");
                        }
                    }
                }
                return jToken;
            }
        }
    }
}