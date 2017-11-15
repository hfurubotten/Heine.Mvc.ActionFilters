using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using NLog;

namespace Heine.Mvc.ActionFilters
{
    public class LogClientErrorsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var statusCode = actionExecutedContext.Response.StatusCode;

            if (statusCode == HttpStatusCode.BadRequest)
            {
                LogManager.GetCurrentClassLogger().Warn("Encountered a bad request from a client. \n" +
                    "Request body: {0} \n" +
                    "Response body: {1}", 
                    GetBody(actionExecutedContext.Request.Content),
                    GetBody(actionExecutedContext.Response.Content));
            }
        }


        private static string GetBody(HttpContent content)
        {
            var requestStream = content.ReadAsStreamAsync().Result;
            requestStream.Position = 0;
            var streamReader = new StreamReader(requestStream, Encoding.UTF8);
            var requestBody = streamReader.ReadToEnd();
            return requestBody;
        }
    }
}
