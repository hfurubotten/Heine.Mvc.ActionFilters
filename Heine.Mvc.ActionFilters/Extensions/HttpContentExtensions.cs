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
        public static string ReadAsString(this HttpContent httpContent, HttpHeaders httpHeaders, bool isHttpResponse)
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
                return
                    $"Unable to read body of HTTP content:\n{string.Join("\n", ex.GetMessages().Select(message => $"- '{message}'"))}";
            }

            string Format(string content)
            {
                switch (httpContent.Headers?.ContentType?.MediaType)
                {
                    case "application/json":
                        try
                        {
                            return Obfuscate(JToken.Parse(content), httpHeaders)
                                .ToString(Formatting.Indented);
                        }
                        catch
                        {
                            return content;
                        }

                    case "application/xml":
                        try
                        {
                            return XDocument.Parse(content).ToString();
                        }
                        catch
                        {
                            return content;
                        }

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
                        // APIs supports both camel case and pascal case.
                        // Trying to first select with pascal case.
                        var tokens = jToken.SelectTokens($"{jPath.Path}.{property}");

                        // Try selecting with camel case if no tokens were found with pascal case.
                        if (!tokens.Any())
                            tokens = jToken.SelectTokens($"{jPath.Path}.{property.JsonPathToCamelCase()}");

                        // Trying to select with alternative path because it could be an OData req/resp.
                        if (!jPath.IsArray && isHttpResponse && !tokens.Any())
                            tokens = jToken.SelectTokens($"{jPath.Path}.value[*].{property.JsonPathToCamelCase()}");

                        foreach (var token in tokens)
                        {
                            if (!token.IsNullOrEmpty())
                            {
                                // JToken can be of type JArray, JObject, JProperty or JValue.
                                if (token.Type == JTokenType.Array)
                                {
                                    foreach (var obj in token)
                                    {
                                        ObfuscateObject(obj);
                                    }
                                }
                                else if (token.Type == JTokenType.Object)
                                {
                                    ObfuscateObject(token);
                                }
                                else
                                {
                                    token.Replace("*** OBFUSCATED ***");
                                }
                            }
                        }
                    }
                }
                return jToken;
            }

            // Obfuscate each property (JProperty) of object (JObject)
            void ObfuscateObject(JToken token)
            {
                // JObject will always only contain properties of type JProperty.
                foreach (var prop in token)
                {
                    if (!prop.IsNullOrEmpty())
                    {
                        // Each property will always have one key/value pair.
                        prop.First.Replace("*** OBFUSCATED ***");
                    }
                }
            }
        }
    }
}