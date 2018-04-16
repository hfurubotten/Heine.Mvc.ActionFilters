using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HeaderUtilities
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static IDictionary<string, Func<string, string>> ObfuscatedHeaders { get; set; } = new Dictionary<string, Func<string, string>>
        {
            {
                "Authorization", headerValue =>
                {
                    if (string.IsNullOrWhiteSpace(headerValue)) return headerValue;
                    if (!headerValue.Contains(' ')) return headerValue.ReplaceEnd('*', 2f / 3f);
                    var headerValueParts = headerValue.Split(new[] { ' ' }, 2);
                    return $"{headerValueParts.First()} {new string('*', headerValueParts.Last().Length < 10 ? headerValueParts.Last().Length : 10)}{(headerValueParts.Last().Length > 10 ? "..." : string.Empty)}";
                }
            }
        };

        // ReSharper disable once CollectionNeverUpdated.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public static ICollection<string> ExcludedHeaders { get; set; } = new List<string>();

        internal static Dictionary<string, string> GetLoggableHeaders(params HttpHeaders[] headersCollections)
        {
            var clone = new Dictionary<string, string>();
            foreach (var headers in headersCollections)
            {
                if (headers == null)
                    continue;

                foreach (var header in headers)
                {
                    if (ExcludedHeaders.Contains(header.Key)) continue;

                    if (ObfuscatedHeaders.ContainsKey(header.Key))
                    {
                        clone.Add(header.Key, ObfuscatedHeaders[header.Key](header.Value?.FirstOrDefault()));
                        continue;
                    }

                    clone.Add(header.Key, string.Join(", ", header.Value));
                }
            }

            return clone;
        }
    }
}