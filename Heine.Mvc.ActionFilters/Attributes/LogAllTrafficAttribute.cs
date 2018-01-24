﻿using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Extensions;
using NLog;

namespace Heine.Mvc.ActionFilters.Attributes
{
    /// <summary>
    ///     Filter for logging all API calls registered on the API.
    /// </summary>
    /// <remarks>
    ///     Placement of this filter is crucial if it should catch information generated by other filters.
    ///     Note: Responses set through exceptions will not be logged through this filter.
    ///     Note 2: Requests stopped before hitting this filter will not be logged.
    /// </remarks>
    public sealed class LogAllTrafficAttribute : ActionFilterAttribute
    {
        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Logger.Debug("Request: {0}", actionContext.Request.AsFormattedString());

            base.OnActionExecuting(actionContext);
        }

        /// <inheritdoc />
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
                if (actionExecutedContext.Response.IsSuccessStatusCode)
                    Logger.Debug("Response: {0}", actionExecutedContext.Response.AsFormattedString());

                else if (actionExecutedContext.Response.StatusCode < HttpStatusCode.InternalServerError)
                    Logger.Warn("Response: {0}", actionExecutedContext.Response.AsFormattedString());

                else
                    Logger.Error("Response: {0}", actionExecutedContext.Response.AsFormattedString());

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}