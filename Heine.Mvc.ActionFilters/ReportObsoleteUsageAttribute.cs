using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NLog;

namespace Heine.Mvc.ActionFilters
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ReportObsoleteUsageAttribute : ActionFilterAttribute
    {
        private static ILogger logger;

        /// <summary>
        /// The error level the message should be reported as. Default value: Warn.
        /// </summary>
        public LogLevel Level = LogLevel.Warn;

        /// <summary>
        /// The message format which will be logged. If the message will be logged the value will go through string format.
        /// Add format variables to get context values in the logging.
        /// Default value: Use of obsolete API method. \n Url: {0} \n Controller Name: {1} \n Action Name: {2}
        /// Format Variables:
        /// {0} : Request URL
        /// {1} : Controller name
        /// {2} : Action name
        /// </summary>
        public string MessageFormat = "Use of obsolete API method. \n Url: {0} \n Controller Name: {1} \n Action Name: {2} ";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<ObsoleteAttribute>().Any())
            {
                if (logger == null)
                    logger = LogManager.GetCurrentClassLogger();

                logger.Log(Level, MessageFormat, actionContext.Request.RequestUri.AbsolutePath,
                    actionContext.ControllerContext.ControllerDescriptor.ControllerName,
                    actionContext.ActionDescriptor.ActionName);
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
