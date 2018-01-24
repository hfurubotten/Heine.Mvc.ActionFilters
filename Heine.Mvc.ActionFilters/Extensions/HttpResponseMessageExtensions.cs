using System.Net.Http;
using System.Text;

namespace Heine.Mvc.ActionFilters.Extensions 
{
    public static class HttpResponseMessageExtensions
    {
        public static string AsFormattedString(this HttpResponseMessage response)
        {
            if (response == null) return "Response: <empty>";

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(response.ToString());

            var stringContent = response.Content?.AsFormattedString();
            if (!string.IsNullOrWhiteSpace(stringContent))
                stringBuilder.AppendLine(stringContent);

            return stringBuilder.ToString();
        }
    }
}