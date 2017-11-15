using System.Net;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Extensions;
using NLog;

namespace Heine.Mvc.ActionFilters
{
    public class LogClientErrorsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var statusCode = actionExecutedContext.Response?.StatusCode;

            if (statusCode == HttpStatusCode.BadRequest)
            {
                LogManager.GetCurrentClassLogger().Warn("Encountered a bad request from a client. \n" +
                    "Request body: {0} \n" +
                    "Response body: {1}", 
                    actionExecutedContext.Request?.Content.GetBody(),
                    actionExecutedContext.Response?.Content.GetBody());
            }
        }
    }
}
