using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Exceptions;
using Heine.Mvc.ActionFilters.Extensions;
using NLog;

namespace Heine.Mvc.ActionFilters.Attributes
{
    public sealed class LogExceptionAttribute : ExceptionFilterAttribute
    {
        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            if (!(exception is HttpStatusException))
                Logger.Error(exception, actionExecutedContext.Request);
        }
    }
}