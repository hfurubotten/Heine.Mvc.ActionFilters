using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters
{
    public class ProcessHttpStatusExceptions : ExceptionFilterAttribute 
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            var actionContext = actionExecutedContext.ActionContext;

            if (exception is HttpStatusException)
            {
                var httpEx = exception as HttpStatusException;
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                        httpEx.HttpCode, httpEx.Message);
            }

        }
    }
}
