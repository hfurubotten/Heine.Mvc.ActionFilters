using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HeaderUtilities
    {
        public static ICollection<string> ObfuscatedHeaders { get; set; } = new List<string>
        {
            "Authorization"
        };

        public static ICollection<string> ExcludedHeaders { get; set; } = new List<string>();

        internal static string DumpHeaders(params HttpHeaders[] headers)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("{\r\n");
            foreach (var header in headers)
            {
                if (header != null)
                {
                    foreach (var keyValuePair in header)
                    {
                        var headerKey = keyValuePair.Key;
                        if (ExcludedHeaders.Contains(headerKey)) continue;

                        foreach (var str in keyValuePair.Value)
                        {
                            var headerValue = str;

                            if (ObfuscatedHeaders.Contains(headerKey))
                            {
                                headerValue = new string('*', 10);
                            }

                            stringBuilder.Append("  ");
                            stringBuilder.Append(headerKey);
                            stringBuilder.Append(": ");
                            stringBuilder.Append(headerValue);
                            stringBuilder.Append("\r\n");
                        }
                    }
                }
            }
            stringBuilder.Append('}');
            return stringBuilder.ToString();
        }

        internal static Dictionary<string, string> CloneHeaders(params HttpHeaders[] headersCollections)
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
                        clone.Add(header.Key, value?.Substring(0, value.Length < 10 ? value.Length : 10) + "...");

                        continue;
                    }

                    clone.Add(header.Key, string.Join(", ", header.Value));
                }
            }

            return clone;
        }
    }
}
