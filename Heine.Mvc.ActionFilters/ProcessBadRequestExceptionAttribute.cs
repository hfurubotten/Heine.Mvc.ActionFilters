using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters
{
    public class ProcessBadRequestExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            var actionContext = actionExecutedContext.ActionContext;

            if (exception is NotFoundException)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.NotFound, exception.Message);
            }
        }
    }
}
