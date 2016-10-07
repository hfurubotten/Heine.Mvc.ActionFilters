using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using NLog;
using System.Net.Http;
using System.Net;


namespace Heine.Mvc.ActionFilters
{
    public sealed class NotFoundExceptionAttribute : ExceptionFilterAttribute
    {
        private static ILogger logger;

        /// <summary>
        /// The message format which will be logged. If the message will be logged the value will go through string format.
        /// Add format variables to get context values in the logging.
        /// Default value: Use of obsolete API method. \n Message: {0} \n Url: {1} \n Controller Name: {2} \n Action Name: {3}
        /// Format Variables:
        /// {0} : Request URL
        /// {1} : Controller name
        /// {2} : Action name
        /// </summary>
        public string MessageFormat = "Object was not found. \n Message: {0} \n Url: {1} \n Controller Name: {2} \n Action Name: {3} ";

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            var actionContext = actionExecutedContext.ActionContext;

            if (exception is NotFoundException)
            {
                if (logger == null)
                    logger = LogManager.GetCurrentClassLogger();

                logger.Error(exception, MessageFormat, actionContext.Request.RequestUri.AbsolutePath,
                    actionContext.ControllerContext.ControllerDescriptor.ControllerName,
                    actionContext.ActionDescriptor.ActionName);

                actionContext.Response = actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.NotFound, exception.Message);
            }
        }
    }
}
