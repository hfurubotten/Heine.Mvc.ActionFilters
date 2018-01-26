using System.Collections.Generic;
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
                                headerValue = new string('*', headerValue.Length);
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
    }
}