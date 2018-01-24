using System.Net.Http;
using System.Text;

namespace Heine.Mvc.ActionFilters.Extensions 
{
    public static class HttpRequestMessageExtensions
    {
        public static string AsFormattedString(this HttpRequestMessage request)
        {
            if (request == null) return "<empty>";

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(request.ToString());

            var stringContent = request.Content?.AsFormattedString();
            if (!string.IsNullOrWhiteSpace(stringContent))
                stringBuilder.AppendLine(stringContent);

            return stringBuilder.ToString();
        }
    }
}