using System.Net.Http;
using System.Text;

namespace Heine.Mvc.ActionFilters.Extensions 
{
    public static class HttpRequestMessageExtensions
    {
        public static string AsFormattedString(this HttpRequestMessage response)
        {
            if (response == null) return string.Empty;

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(response.ToString());

            if (response.Content != null)
            {
                stringBuilder.AppendLine(response.Content.AsFormattedString());
            }

            return stringBuilder.ToString();
        }
    }
}