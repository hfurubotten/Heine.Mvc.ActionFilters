using System;
using System.Net;

namespace Heine.Mvc.ActionFilters
{
    public class ConflictException : HttpStatusException
    {
        public ConflictException()
            : base(HttpStatusCode.Conflict, string.Empty)
        {
        }

        public ConflictException(string message)
            : base(HttpStatusCode.Conflict, message)
        {
        }

        public ConflictException(string message, Exception inner)
            : base(HttpStatusCode.Conflict, message, inner)
        {
        }
    }
}