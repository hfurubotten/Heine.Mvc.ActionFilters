using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters.Tests
{
    public static class HttpActionContextFactory
    {
        public static HttpActionContext GetActionContext()
        {
            return GetActionContext(new HttpRequestMessage());
        }

        public static HttpActionContext GetActionContext(HttpRequestMessage request)
        {
            return new HttpActionContext(new HttpControllerContext { Request = request }, new ReflectedHttpActionDescriptor());
        }

        public static HttpActionContext GetActionContext(HttpRequestMessage request, HttpResponseMessage response)
        {
            return new HttpActionContext(new HttpControllerContext { Request = request }, new ReflectedHttpActionDescriptor())
            {
                Response = response
            };
        }

        public static HttpActionExecutedContext GetActionExecutedContext(Exception exception)
        {
            return new HttpActionExecutedContext
            {
                ActionContext = GetActionContext(),
                Exception = exception
            };
        }
    }
}