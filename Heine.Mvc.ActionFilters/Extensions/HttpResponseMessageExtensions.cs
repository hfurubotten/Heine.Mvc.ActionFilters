using System.Collections.Generic;
using System.Net.Http;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static (int statusCode, string reasonPhrase, string version, string contentTypeName, Dictionary<string, string> headers, string body) Destruct(this HttpResponseMessage response)
        {
            return (
                (int) response.StatusCode,
                response.ReasonPhrase,
                response.Version?.ToString(),
                response.Content?.GetType().FullName,
                HeaderUtilities.GetLoggableHeaders(response.Headers, response.Content?.Headers),
                response.Content?.ReadAsString()
            );
        }
    }
}