using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);

            //base.OnActionExecuting(actionContext);
        }

        //public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        //{
        //    if (!actionContext.ModelState.IsValid)
        //        actionContext.Response = actionContext.Request.CreateErrorResponse(
        //            HttpStatusCode.BadRequest, actionContext.ModelState);

        //    return base.OnActionExecutingAsync(actionContext, cancellationToken);
        //}
    }
}
