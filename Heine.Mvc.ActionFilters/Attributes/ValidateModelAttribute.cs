using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Extensions;
using Newtonsoft.Json;
using NLog;

namespace Heine.Mvc.ActionFilters.Attributes
{
    public sealed class ValidateModelAttribute : ActionFilterAttribute
    {
        public bool LogModelErrors = true;

        private ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid) return;

            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);

            if (LogModelErrors)
                LoggerExtensions.Warn(
                    Logger, 
                    actionContext.Request.AsFormattedString(), 
                    JsonConvert.SerializeObject(actionContext.ModelState, Formatting.Indented), 
                    "Model state on client request is invalid.");
        }
    }
}