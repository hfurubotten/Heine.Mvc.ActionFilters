using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Exceptions;
using Heine.Mvc.ActionFilters.Extensions;
using Heine.Mvc.ActionFilters.Interfaces;
using NLog;

namespace Heine.Mvc.ActionFilters.ExceptionFilterAttributes
{
    public sealed class LogExceptionAttribute : ExceptionFilterAttribute, IOrderableFilter
    {
        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger(typeof(LogExceptionAttribute));

        public int Order { get; set; }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            if (!(exception is HttpStatusException))
                Logger.Error(exception, actionExecutedContext.Request);
        }
    }
}