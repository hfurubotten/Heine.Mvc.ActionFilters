using System.Net.Http;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Exceptions;
using Heine.Mvc.ActionFilters.Extensions;
using Heine.Mvc.ActionFilters.Interfaces;
using NLog;

namespace Heine.Mvc.ActionFilters.ExceptionFilterAttributes
{
    /// <remarks>
    ///     Make sure the delegate cannot fail with an unhandled exception.
    /// </remarks>
    public delegate void OnActionExecutedDelegate(HttpActionExecutedContext actionExecutedContext);

    public sealed class ProcessHttpStatusExceptionsAttribute : ExceptionFilterAttribute, IOrderableFilter
    {
        private readonly OnActionExecutedDelegate[] onActionExecutedDelegates;
        public bool ShouldLog = true;

        public ProcessHttpStatusExceptionsAttribute()
        {
            onActionExecutedDelegates = new OnActionExecutedDelegate[0];
        }

        public ProcessHttpStatusExceptionsAttribute(params OnActionExecutedDelegate[] onActionExecutedDelegates)
        {
            this.onActionExecutedDelegates = onActionExecutedDelegates ?? new OnActionExecutedDelegate[0];
        }

        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger(typeof(ProcessHttpStatusExceptionsAttribute));

        public int Order { get; set; }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            if (exception is HttpStatusException)
            {
                var httpEx = exception as HttpStatusException;

                if (string.IsNullOrWhiteSpace(httpEx.Message))
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(httpEx.StatusCode);
                    actionExecutedContext.ActionContext.Response = actionExecutedContext.Request.CreateResponse(httpEx.StatusCode);
                }
                else
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(httpEx.StatusCode, httpEx.Message);
                    actionExecutedContext.ActionContext.Response = actionExecutedContext.Request.CreateErrorResponse(httpEx.StatusCode, httpEx.Message);
                }

                foreach (var onActionExecutedDelegate in onActionExecutedDelegates) onActionExecutedDelegate(actionExecutedContext);

                if (ShouldLog)
                    Logger.Log(httpEx.LogLevel, actionExecutedContext.Request, actionExecutedContext.Response);
            }

            base.OnException(actionExecutedContext);
        }
    }
}