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

        public RequestResponseCorrelationAttribute(string headerKey, Func<string> defaultValue)
        {
            this.headerKey = headerKey;
            this.defaultValue = defaultValue;
        }

        public RequestResponseCorrelationAttribute(string headerKey, string defaultValue) : this(headerKey, () => defaultValue) { }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext?.Request?.Headers?.Contains(headerKey) == true) return;

            actionContext.Request.Headers.Add(headerKey, defaultValue());
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext?.Response?.Headers?.Contains(headerKey) == true) return;

            actionExecutedContext.Response.Headers.Add(headerKey, actionExecutedContext.Request.Headers.GetValues(headerKey));
        }

        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            OnActionExecuted(actionExecutedContext);

            return Task.FromResult(0);
        }
    }
}