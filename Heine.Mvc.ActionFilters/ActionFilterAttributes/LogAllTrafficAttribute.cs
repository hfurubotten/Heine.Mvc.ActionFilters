﻿using System.Net;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Extensions;
using Heine.Mvc.ActionFilters.Interfaces;
using NLog;

namespace Heine.Mvc.ActionFilters.ActionFilterAttributes
{
    /// <summary>
    ///     Filter for logging all API calls registered on the API.
    /// </summary>
    /// <remarks>
    ///     Placement of this filter is crucial if it should catch information generated by other filters.
    ///     Note: Responses set through exceptions will not be logged through this filter.
    ///     Note 2: Requests stopped before hitting this filter will not be logged.
    /// </remarks>
    public sealed class LogAllTrafficAttribute : ActionFilterAttribute, IOrderableFilter
    {
        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger(typeof(LogAllTrafficAttribute));

        public int Order { get; set; }
        
        /// <inheritdoc />
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
            {
                Logger.Debug("Request: {0}", actionExecutedContext.Request.AsFormattedString());

                if (actionExecutedContext.Response.IsSuccessStatusCode)
                    Logger.Debug("Response: {0}", actionExecutedContext.Response.AsFormattedString());

                else if (actionExecutedContext.Response.StatusCode < HttpStatusCode.InternalServerError)
                    Logger.Warn("Response: {0}", actionExecutedContext.Response.AsFormattedString());

                else
                    Logger.Error("Response: {0}", actionExecutedContext.Response.AsFormattedString());
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}