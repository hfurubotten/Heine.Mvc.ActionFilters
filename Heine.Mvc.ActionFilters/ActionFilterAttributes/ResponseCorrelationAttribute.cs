using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Interfaces;

namespace Heine.Mvc.ActionFilters.ActionFilterAttributes
{
    public class ResponseCorrelationAttribute : ActionFilterAttribute, IOrderableFilter, IExceptionFilter
    {
        private readonly string headerKey;

        public ResponseCorrelationAttribute() : this("X-CorrelationID") { }

        public ResponseCorrelationAttribute(string headerKey)
        {
            this.headerKey = headerKey;
        }

        public int Order { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext) { }

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