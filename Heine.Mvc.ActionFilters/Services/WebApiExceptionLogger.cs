using System.Web.Http.ExceptionHandling;
using Heine.Mvc.ActionFilters.Exceptions;
using Heine.Mvc.ActionFilters.Extensions;
using NLog;

namespace Heine.Mvc.ActionFilters.Services
{
    public class WebApiExceptionLogger : ExceptionLogger
    {
        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     The maximum number of charachters from the request body that will be logged
        /// </summary>
        /// <remarks>
        ///     Default is 1000.
        ///     Set to null to disable truncation
        /// </remarks>
        public int? RequestBodyMaxLogLength { get; set; } = 10000;

        public override void Log(ExceptionLoggerContext context)
        {
            if (context?.Exception == null)
                return;

            if (context.Exception is HttpStatusException)
                return;
            
            if (RequestBodyMaxLogLength.HasValue)
                Logger.Error(context.Exception, context.Request, RequestBodyMaxLogLength.Value);
            else
                Logger.Error(context.Exception, context.Request);

            base.Log(context);
        }
    }
}