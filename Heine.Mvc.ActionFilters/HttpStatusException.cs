using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Heine.Mvc.ActionFilters
{
    public class HttpStatusException : Exception
    {
        public HttpStatusCode HttpCode { get; set; }

        public HttpStatusException() : base()
        {
            HttpCode = HttpStatusCode.InternalServerError;
        }

        public HttpStatusException(HttpStatusCode httpCode) : base()
        {
            HttpCode = HttpCode;
        }

        public HttpStatusException(HttpStatusCode httpCode, string message)
            : base(message)
        {
            HttpCode = HttpCode;
        }

        public HttpStatusException(HttpStatusCode httpCode, string message, Exception inner)
            : base(message, inner)
        {
            HttpCode = HttpCode;
        }
    }
}
