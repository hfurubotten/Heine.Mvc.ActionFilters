using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters
{
    public class ProcessHttpStatusExceptionsAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            var actionContext = actionExecutedContext.ActionContext;

            if (exception is HttpStatusException)
            {
                var httpEx = exception as HttpStatusException;

                if (string.IsNullOrWhiteSpace(httpEx.Message))
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(httpEx.HttpCode);
                    actionExecutedContext.ActionContext.Response = actionExecutedContext.Request.CreateResponse(httpEx.HttpCode);
                }
                else
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                               httpEx.HttpCode, httpEx.Message);

                    actionExecutedContext.ActionContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                        httpEx.HttpCode, httpEx.Message);
                }
            }

            base.OnException(actionExecutedContext);
        }
    }
}
