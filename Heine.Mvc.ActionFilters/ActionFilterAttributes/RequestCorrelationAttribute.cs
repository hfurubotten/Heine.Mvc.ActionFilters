using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Interfaces;

namespace Heine.Mvc.ActionFilters.ActionFilterAttributes
{
    public class RequestCorrelationAttribute : ActionFilterAttribute, IOrderableFilter
    {
        private readonly Func<string> defaultValue;
        private readonly string headerKey;

        public RequestCorrelationAttribute() : this("X-CorrelationID", () => Guid.NewGuid().ToString()) { }

        public RequestCorrelationAttribute(string headerKey, string defaultValue) : this(headerKey, () => defaultValue) { }

        public RequestCorrelationAttribute(string headerKey, Func<string> defaultValue)
        {
            this.headerKey = headerKey;
            this.defaultValue = defaultValue;
        }

        public int Order { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext?.Request?.Headers == null) return;
            if (actionContext.Request.Headers.Contains(headerKey)) return;

            actionContext.Request.Headers.Add(headerKey, defaultValue());
        }
    }
}