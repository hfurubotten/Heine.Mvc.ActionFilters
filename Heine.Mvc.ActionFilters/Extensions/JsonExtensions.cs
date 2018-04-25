using System;
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
    }
}