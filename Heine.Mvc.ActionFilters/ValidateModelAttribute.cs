using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Extensions;
using Newtonsoft.Json;
using NLog;

namespace Heine.Mvc.ActionFilters
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public sealed class ValidateModelAttribute : ActionFilterAttribute
    {
        public bool LogModelErrors = true;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);

                if(LogModelErrors)
                    LogManager.GetCurrentClassLogger().Warn("Modelstate on client request is invalid. \n" +
                        "Request: {0} \n" +
                        "Response: {1}", 
                        actionContext.Request?.Content.GetBody(),
                        JsonConvert.SerializeObject(actionContext.ModelState, Formatting.Indented));
            }
        }
    }
}