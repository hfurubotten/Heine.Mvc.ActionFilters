using System;
using System.Net;

namespace Heine.Mvc.ActionFilters
{
    public class BadRequestException : HttpStatusException
    {
        public BadRequestException()
            : base(HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException(string message)
            : base(HttpStatusCode.BadRequest, message)
        {
        }

        public BadRequestException(string message, Exception inner)
            : base(HttpStatusCode.BadRequest, message, inner)
        {
        }
    }
}
