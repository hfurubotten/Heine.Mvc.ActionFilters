using NLog;
using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters
{
    public sealed class LogExceptionAttribute : ExceptionFilterAttribute
    {
        private static ILogger logger;

        /// <summary>
        /// The message format which will be logged. If the message will be logged the value will go through string format.
        /// Add format variables to get context values in the logging.
        /// Default value: Use of obsolete API method. \n Url: {0} \n Controller Name: {1} \n Action Name: {2}
        /// Format Variables:
        /// {0} : Request URL
        /// {1} : Controller name
        /// {2} : Action name
        /// </summary>
        public string MessageFormat = "An exception occured. \n Url: {0} \n Controller Name: {1} \n Action Name: {2} ";

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            var actionContext = actionExecutedContext.ActionContext;

            if (logger == null)
                logger = LogManager.GetCurrentClassLogger();

            logger.Error(exception, MessageFormat, actionContext.Request.RequestUri.AbsolutePath,
                actionContext.ControllerContext.ControllerDescriptor.ControllerName,
                actionContext.ActionDescriptor.ActionName);
        }
    }
}