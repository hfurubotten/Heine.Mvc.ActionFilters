using System.Net.Http;
using System.Text;

namespace Heine.Mvc.ActionFilters.Extensions 
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage Clone(this HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = request.Content.Clone(),
                Version = request.Version
            };
            foreach (var prop in request.Properties)
            {
                clone.Properties.Add(prop);
            }
            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        public static string AsFormattedString(this HttpRequestMessage request)
        {
            if (request == null) return "<empty>";

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(ToString(request));

            var stringContent = request.Content?.AsFormattedString();
            if (!string.IsNullOrWhiteSpace(stringContent))
                stringBuilder.AppendLine(stringContent);

            return stringBuilder.ToString();
        }

        public static string ToString(this HttpRequestMessage request)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Method: ");
            stringBuilder.Append(request.Method);
            stringBuilder.Append(", RequestUri: '");
            stringBuilder.Append(request.RequestUri == null ? "<null>" : request.RequestUri.ToString());
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