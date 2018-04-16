using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HeaderUtilities
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static ICollection<string> ObfuscatedHeaders { get; set; } = new List<string>
        {
            "Authorization"
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

                    if (ObfuscatedHeaders.Contains(header.Key))
                    {
                        var value = header.Value?.FirstOrDefault();

                        clone.Add(header.Key, 
                            string.IsNullOrEmpty(value)
                                ? string.Empty
                                : value.Length > 10 
                                    ? $"{value.Substring(0, 10)}..." 
                                    : value.ReplaceEnd('.', 2f / 3f));

                        continue;
                    }

                    clone.Add(header.Key, string.Join(", ", header.Value));
                }
            }
            return clone;
        }
    }
}
