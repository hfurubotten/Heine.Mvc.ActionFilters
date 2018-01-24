using System.Net.Http;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Exceptions;
using Heine.Mvc.ActionFilters.Extensions;
using NLog;

namespace Heine.Mvc.ActionFilters.Attributes
{
    public sealed class ProcessHttpStatusExceptionsAttribute : ExceptionFilterAttribute
    {
        public bool ShouldLog = true;

        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

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

                if (ShouldLog)
                    Logger.Log(httpEx.LogLevel, actionExecutedContext.Request, actionExecutedContext.Response);
            }

            base.OnException(actionExecutedContext);
        }
    }
}