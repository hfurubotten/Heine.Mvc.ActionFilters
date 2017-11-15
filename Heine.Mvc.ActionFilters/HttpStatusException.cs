using System;
using System.Net;

namespace Heine.Mvc.ActionFilters
{
    public class HttpStatusException : Exception
    {
        public HttpStatusCode HttpCode { get; set; }

        public HttpStatusException() : base(string.Empty)
        {
            HttpCode = HttpStatusCode.InternalServerError;
        }

        public HttpStatusException(HttpStatusCode httpCode) : base(string.Empty)
        {
            HttpCode = httpCode;
        }

        public HttpStatusException(HttpStatusCode httpCode, string message)
            : base(message)
        {
            HttpCode = httpCode;
        }

        public HttpStatusException(HttpStatusCode httpCode, string message, Exception inner)
            : base(message, inner)
        {
            HttpCode = httpCode;
        }
    }
}
