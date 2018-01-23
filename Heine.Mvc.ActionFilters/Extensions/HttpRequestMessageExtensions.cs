using System.Net.Http;
using System.Text;

namespace Heine.Mvc.ActionFilters.Extensions 
{
    public static class HttpRequestMessageExtensions
    {
        public static string AsFormattedString(this HttpRequestMessage request)
        {
            if (request == null) return string.Empty;

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(request.ToString());

            if (request.Content != null)
            {
                stringBuilder.AppendLine(request.Content.AsFormattedString());
            }

            return stringBuilder.ToString();
        }
    }
}