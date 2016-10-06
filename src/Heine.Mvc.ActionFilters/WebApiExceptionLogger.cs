using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Heine.Mvc.ActionFilters
{
    public class WebApiExceptionLogger : ExceptionLogger
    {
        protected static ILogger logger;

        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                if (logger == null)
                    logger = LogManager.GetCurrentClassLogger();

                logger.Error(context.Exception, "Unhandled exception found on 500 http status code.");
            }

            base.Log(context);
        }
    }
}
