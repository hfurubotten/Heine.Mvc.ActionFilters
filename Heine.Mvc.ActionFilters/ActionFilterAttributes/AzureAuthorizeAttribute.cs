using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Heine.Mvc.ActionFilters.ActionFilterAttributes
{
    public class AzureAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentNullException(nameof(actionContext));

            var principal = (ClaimsPrincipal) actionContext.ControllerContext.RequestContext.Principal;

            var usersSplit = new List<string>();
            var rolesSplit = new List<string>();

            if (!string.IsNullOrWhiteSpace(Users))
                usersSplit = Users.Split(',').ToList();
            if (!string.IsNullOrWhiteSpace(Roles))
                rolesSplit = Roles.Split(',').ToList();

            return principal?.Identity != null &&
                principal.Identity.IsAuthenticated &&
                (
                    usersSplit.Count == 0 || usersSplit.Contains(principal.Identity.Name, StringComparer.OrdinalIgnoreCase)
                ) &&
                (
                    rolesSplit.Count == 0 || rolesSplit.Any(x => principal.HasClaim("roles", x))
                );
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentNullException(nameof(actionContext));

            var principal = actionContext.ControllerContext.RequestContext.Principal;

            if (principal?.Identity != null && principal.Identity.IsAuthenticated)
            {
                var errorMessage = "You are not authorized to use this endpoint.";
                if (Roles.Split(',').Length > 1)
                    errorMessage += $" At least one of the following roles are required: {Roles.Replace(",", ", ")}";
                else
                    errorMessage += $" The following role is required: {Roles}";

                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, errorMessage);
            }
            else
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
        }
    }
}