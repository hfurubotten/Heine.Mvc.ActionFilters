using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using NLog;
using System.Web.Http.ExceptionHandling;

namespace Heine.Mvc.ActionFilters
{
    public class WebApiExceptionLogger : ExceptionLogger
    {
        /// <summary>
        /// The maximum number of charachters from the request body that will be logged
        /// </summary>
        /// <remarks>
        /// Default is 1000
        /// </remarks>
        public int RequestBodyMaxLogLength { get; set; } = 10000;
        protected static ILogger Logger;

        public override void Log(ExceptionLoggerContext context)
        {
            if (context?.Exception == null)
                return;

            if (context.Exception is HttpStatusException)
                return;

            if (Logger == null)
                Logger = LogManager.GetCurrentClassLogger();

            var requestBody = GetRequestBody(context.Request);
            var prettyfiedBody = requestBody.PrettyPrint();

            // Truncate large requests to prevent bloating log
            if (prettyfiedBody.Length > RequestBodyMaxLogLength)
                prettyfiedBody = new string(prettyfiedBody.Take(RequestBodyMaxLogLength).ToArray()) + "...";

            if (context.Exception.InnerException != null)
            {
                Logger.Error(context.Exception,
                    "An error occured: {0}\nInner exception: {1}\n\nRequest details:\nMethod: {2}\nURI: {3}\nBody: {4}",
                    context.Exception.Message, context.Exception.InnerException?.Message, context.Request.Method.Method,
                    context.Request.RequestUri.PathAndQuery, prettyfiedBody);
            }
            else
            {
                Logger.Error(context.Exception,
                    "An error occured: {0}\n\n\nRequest details:\nMethod: {1}\nURI: {2}\nBody: {3}",
                    context.Exception.Message, context.Request.Method.Method, context.Request.RequestUri.PathAndQuery, prettyfiedBody);
            }

            base.Log(context);
        }

        private static string GetRequestBody(HttpRequestMessage message)
        {
            var requestStream = message.Content.ReadAsStreamAsync().Result;
            requestStream.Position = 0;
            var streamReader = new StreamReader(requestStream, Encoding.UTF8);
            var requestBody = streamReader.ReadToEnd();
            return requestBody;
        }
    }
}