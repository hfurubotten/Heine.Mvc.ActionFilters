using System.Net.Http;
using System.Text;

namespace Heine.Mvc.ActionFilters.Extensions 
{
    public static class HttpResponseMessageExtensions
    {
        public static string AsFormattedString(this HttpResponseMessage response)
        {
            if (response == null) return "<empty>";

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(ToString(response));

            var stringContent = response.Content?.AsFormattedString();
            if (!string.IsNullOrWhiteSpace(stringContent))
                stringBuilder.AppendLine(stringContent);

            return stringBuilder.ToString();
        }

        public static string ToString(this HttpResponseMessage request)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("StatusCode: ");
            stringBuilder.Append((int) request.StatusCode);
            stringBuilder.Append(", ReasonPhrase: '");
            stringBuilder.Append(request.ReasonPhrase ?? "<null>");
            stringBuilder.Append("', Version: ");
            stringBuilder.Append(request.Version);
            stringBuilder.Append(", Content: ");
            stringBuilder.Append(request.Content == null ? "<null>" : request.Content.GetType().FullName);
            stringBuilder.Append(", Headers:\r\n");
            stringBuilder.Append(HeaderUtilities.DumpHeaders(request.Headers, request.Content?.Headers));
            return stringBuilder.ToString();
        }
    }
}