using System.Net.Http;
using System.Web.Http.Filters;
using Heine.Mvc.ActionFilters.Extensions;
using Newtonsoft.Json;
using NLog;

namespace Heine.Mvc.ActionFilters
{
    public class ProcessHttpStatusExceptionsAttribute : ExceptionFilterAttribute
    {
        public bool LogStatusCodes = true;

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            if (exception is HttpStatusException)
            {
                var httpEx = exception as HttpStatusException;

                if (string.IsNullOrWhiteSpace(httpEx.Message))
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(httpEx.HttpCode);
                    actionExecutedContext.ActionContext.Response = actionExecutedContext.Request.CreateResponse(httpEx.HttpCode);
                }
                else
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                        httpEx.HttpCode, httpEx.Message);

                    actionExecutedContext.ActionContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                        httpEx.HttpCode, httpEx.Message);
                }
                
                if (LogStatusCodes)
                    LogManager.GetCurrentClassLogger().Warn("Api has asked to return a specific statuscode to the client. \n" +
                        "Status Code: {2}\n" +
                        "Request: {0} \n" +
                        "Response: {1}",
                        actionExecutedContext.Request?.Content?.GetBody(),
                        JsonConvert.SerializeObject(actionExecutedContext.Response?.Content?.GetBody(), Formatting.Indented), 
                        httpEx.HttpCode);
            }

            base.OnException(actionExecutedContext);
        }
    }
}