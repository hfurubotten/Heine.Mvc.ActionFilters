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
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                else
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(
                               HttpStatusCode.BadRequest, exception.Message);
                }
            }

            base.OnException(actionExecutedContext);
        }

        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return Task.Run(() => { OnException(actionExecutedContext); });
        }
    }
}
