﻿using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NLog;

namespace Heine.Mvc.ActionFilters
{
    /// <summary>
    /// Filter for logging all api calls registered on the api. 
    /// </summary>
    /// <remarks>
    /// Placement of this filter is crucial if it should catch information generated by other filters.
    /// Note: Responses set through exceptions will not be logged through this filter.
    /// Note 2: Requests stopped before hitting this filter will not be logged. 
    /// </remarks>
    public class LogAllTrafficAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            LogManager.GetCurrentClassLogger().Debug("API call registered.\n" +
                "{0} {1}\n" +
                "{2}",
                actionContext.Request.Method.Method,
                actionContext.Request.RequestUri.PathAndQuery,
                string.Join("\n", actionContext.Request.Headers.Select(x => $"{x.Key}: {string.Join("; ", x.Value)}")));

            base.OnActionExecuting(actionContext);
        }

        /// <inheritdoc />
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
                LogManager.GetCurrentClassLogger().Debug("API Responding. \n" +
                    "{0} {1} {2}\n" +
                    "{3}",
                    (int)actionExecutedContext.Response.StatusCode,
                    actionExecutedContext.Response.StatusCode,
                    actionExecutedContext.Request?.RequestUri?.PathAndQuery,
                    string.Join("\n", actionExecutedContext.Response.Headers.Select(x => $"{x.Key}: {x.Value}")));

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
