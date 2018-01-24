﻿using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Extensions;
using NLog;

namespace Heine.Mvc.ActionFilters.Attributes
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ReportObsoleteUsageAttribute : ActionFilterAttribute
    {
        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The error level the message should be reported as. Default value: Warn.
        /// </summary>
        public LogLevel Level = LogLevel.Warn;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<ObsoleteAttribute>().Any())
            {
                Logger.Log(Level, actionContext.Request, null, "Use of obsolete API method.");
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
