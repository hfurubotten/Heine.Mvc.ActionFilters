using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Extensions;
using Heine.Mvc.ActionFilters.Interfaces;
using NLog;

namespace Heine.Mvc.ActionFilters.ActionFilterAttributes
{
    /// <summary>
    /// </summary>
    public sealed class ReportObsoleteUsageAttribute : ActionFilterAttribute, IOrderableFilter
    {
        /// <summary>
        ///     The error level the message should be reported as. Default value: Warn.
        /// </summary>
        public LogLevel Level = LogLevel.Warn;

        private ILogger Logger { get; } = LogManager.GetLogger(typeof(ReportObsoleteUsageAttribute).FullName);

        public int Order { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<ObsoleteAttribute>().Any()) Logger.Log(Level, actionContext.Request, null, "Use of obsolete API method.");

            base.OnActionExecuting(actionContext);
        }
    }
}