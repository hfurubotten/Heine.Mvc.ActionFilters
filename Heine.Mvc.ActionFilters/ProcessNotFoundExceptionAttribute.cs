using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters
{
    public sealed class ProcessNotFoundExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            if (exception is NotFoundException)
            {
                actionExecutedContext.ActionContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.NotFound);
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.NotFound);
            }

            base.OnException(actionExecutedContext);
        }
    }
}
