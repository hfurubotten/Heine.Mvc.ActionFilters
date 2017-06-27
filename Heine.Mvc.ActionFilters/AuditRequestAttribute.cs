using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters
{
    public sealed class AuditRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string userName = null;
            if (HttpContext.Current?.User.Identity.IsAuthenticated == true)
            {
                userName = HttpContext.Current.User.Identity.Name;
            }

            if (string.IsNullOrWhiteSpace(userName))
                return;

            foreach (var auditable in actionContext.ActionArguments.Values.OfType<IAuditable>())
            {
                auditable.Audit(userName);
            }
        }
    }
}