using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Extensions;
using Heine.Mvc.ActionFilters.Interfaces;
using NLog;

namespace Heine.Mvc.ActionFilters.ActionFilterAttributes
{
    public sealed class ValidateModelAttribute : ActionFilterAttribute, IOrderableFilter
    {
        public bool LogModelErrors = true;

        private ILogger Logger { get; } = LogManager.GetLogger(typeof(ValidateModelAttribute).FullName);

        public int Order { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid) return;

            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);

            if (LogModelErrors)
                Logger.Warn(
                    actionContext.Request,
                    actionContext.Response,
                    "Model state on client request is invalid.\n");
        }
    }
}