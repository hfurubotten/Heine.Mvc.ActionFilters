using System;
using System.Net;

namespace Heine.Mvc.ActionFilters.Exceptions
{
    public class BadRequestException : HttpStatusException
    {
        public BadRequestException() : base(HttpStatusCode.BadRequest, string.Empty) { }

        public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message) { }

        public BadRequestException(string message, Exception inner) : base(HttpStatusCode.BadRequest, message, inner) { }
    }
}