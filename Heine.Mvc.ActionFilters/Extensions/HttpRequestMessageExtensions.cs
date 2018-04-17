using System.Collections.Generic;
using System.Net.Http;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static (string method, string requestUri, string version, string contentTypeName, Dictionary<string, string> headers, string body) Destruct(this HttpRequestMessage request)
        {
            return (
                request.Method?.Method,
                request.RequestUri?.ToString(),
                request.Version?.ToString(),
                request.Content?.GetType().FullName,
                HeaderUtilities.GetLoggableHeaders(request.Headers, request.Content?.Headers),
                request.Content?.ReadAsString(request.Headers)
            );
        }
    }
}