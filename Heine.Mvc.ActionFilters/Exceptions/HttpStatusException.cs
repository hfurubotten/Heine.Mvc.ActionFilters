using System;
using System.Net;
using NLog;

namespace Heine.Mvc.ActionFilters.Exceptions
{
    public class HttpStatusException : Exception
    {
        public HttpStatusException() : this(HttpStatusCode.InternalServerError, string.Empty) { }

        public HttpStatusException(HttpStatusCode statusCode) : this(statusCode, string.Empty) { }

        public HttpStatusException(HttpStatusCode statusCode, string message, Exception inner = null) : base(message, inner)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Warn;
    }
}