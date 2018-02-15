using System.Net;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Extensions;
using Heine.Mvc.ActionFilters.Interfaces;
using NLog;

namespace Heine.Mvc.ActionFilters.ActionFilterAttributes
{
    public sealed class LogClientErrorsAttribute : ActionFilterAttribute, IOrderableFilter
    {
        private ILogger Logger { get; } = LogManager.GetLogger(typeof(LogClientErrorsAttribute).FullName);

        public int Order { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var statusCode = actionExecutedContext.Response?.StatusCode;

            if (statusCode >= HttpStatusCode.BadRequest && statusCode < HttpStatusCode.InternalServerError) Logger.Warn(actionExecutedContext.Request, actionExecutedContext.Response);
        }
    }
}