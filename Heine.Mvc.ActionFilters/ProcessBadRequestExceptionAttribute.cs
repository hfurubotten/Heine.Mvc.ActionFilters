using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters
{
    public class ProcessBadRequestExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            if (exception is BadRequestException)
            {
                if (string.IsNullOrWhiteSpace(exception.Message))
                {
                    actionExecutedContext.ActionContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest);
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest);

                }
                else
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                               HttpStatusCode.BadRequest, exception.Message);
                    actionExecutedContext.ActionContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest, exception.Message);
                }
            }

            base.OnException(actionExecutedContext);
        }
    }
}
