using System.Net;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Extensions;
using NLog;

namespace Heine.Mvc.ActionFilters.Attributes
{
    public sealed class LogClientErrorsAttribute : ActionFilterAttribute
    {
        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var statusCode = actionExecutedContext.Response?.StatusCode;

            if (statusCode >= HttpStatusCode.BadRequest && statusCode < HttpStatusCode.InternalServerError)
            {
                Logger.Warn(actionExecutedContext.Request, actionExecutedContext.Response);
            }
        }
    }
}
