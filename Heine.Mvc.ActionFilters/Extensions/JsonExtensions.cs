using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class JsonExtensions
    {
        public static bool IsNullOrEmpty(this JToken token) =>
            token == null ||
            token.Type == JTokenType.Array && !token.HasValues ||
            token.Type == JTokenType.Object && !token.HasValues ||
            token.Type == JTokenType.String && string.IsNullOrWhiteSpace(token.ToString()) ||
            token.Type == JTokenType.Null;

        private static IEnumerable<JToken> CaseInsensitiveSelectPropertyValues(this JToken token, string name)
        {
            var obj = token as JObject;
            if (obj == null)
                yield break;
            foreach (var property in obj.Properties())
            {
                if (name == null)
                    yield return property.Value;
                else if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase))
                    yield return property.Value;
            }
        }

        public static IEnumerable<JToken> CaseInsensitiveSelectPropertyValues(this IEnumerable<JToken> tokens,
            string name)
        {
            if (tokens == null)
                throw new ArgumentNullException();

            return tokens.SelectMany(t => t.CaseInsensitiveSelectPropertyValues(name));
        }

        public static JToken CaseInsensitiveSelectPropertyValue(this JToken token, string name)
        {
            var obj = token as JObject;
            return obj?.GetValue(name, StringComparison.OrdinalIgnoreCase);
        }
    }
}