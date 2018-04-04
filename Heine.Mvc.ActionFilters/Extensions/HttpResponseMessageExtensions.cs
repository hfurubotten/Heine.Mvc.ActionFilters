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

        public static string ToString(this HttpResponseMessage response)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("StatusCode: ");
            stringBuilder.Append((int) response.StatusCode);
            stringBuilder.Append(", ReasonPhrase: '");
            stringBuilder.Append(response.ReasonPhrase ?? "<null>");
            stringBuilder.Append("', Version: ");
            stringBuilder.Append(response.Version);
            stringBuilder.Append(", Content: ");
            stringBuilder.Append(response.Content == null ? "<null>" : response.Content.GetType().FullName);
            stringBuilder.Append(", Headers:\r\n");
            stringBuilder.Append(HeaderUtilities.DumpHeaders(response.Headers, response.Content?.Headers));
            return stringBuilder.ToString();
        }
    }
}