using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters.Attributes
{
    public class RequestResponseCorrelationAttribute : ActionFilterAttribute, IExceptionFilter
    {
        private readonly string headerKey;
        private readonly Func<string> defaultValue;

        public RequestResponseCorrelationAttribute() : this("X-CorrelationID", () => Guid.NewGuid().ToString()) { }

        public RequestResponseCorrelationAttribute(string headerKey, string defaultValue) : this(headerKey, () => defaultValue) { }

        public RequestResponseCorrelationAttribute(string headerKey, Func<string> defaultValue)
        {
            this.headerKey = headerKey;
            this.defaultValue = defaultValue;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext?.Request?.Headers == null) return;
            if (actionContext.Request.Headers.Contains(headerKey)) return;

            actionContext.Request.Headers.Add(headerKey, defaultValue());
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext?.Request?.Headers == null) return;
            if (actionExecutedContext.Response?.Headers == null) return;

            if (!actionExecutedContext.Request.Headers.Contains(headerKey)) return;
            if (actionExecutedContext.Response.Headers.Contains(headerKey)) return;

            actionExecutedContext.Response.Headers.Add(headerKey, actionExecutedContext.Request.Headers.GetValues(headerKey));
        }

        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            OnActionExecuted(actionExecutedContext);

            return Task.FromResult(0);
        }
    }
}