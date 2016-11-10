using System;
using System.Net;

namespace Heine.Mvc.ActionFilters
{
    public class NotFoundException : HttpStatusException
    {
        public NotFoundException()
            : base(HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message)
        {
        }

        public NotFoundException(string message, Exception inner)
            : base(HttpStatusCode.NotFound, message, inner)
        {
        }
    }
}
