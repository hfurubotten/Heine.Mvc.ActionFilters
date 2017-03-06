using System.IO;
using System.Linq;
using System.Text;
using NLog;
using System.Web.Http.ExceptionHandling;

namespace Heine.Mvc.ActionFilters
{
    public class WebApiExceptionLogger : ExceptionLogger
    {
        protected static ILogger Logger;

        public override void Log(ExceptionLoggerContext context)
        {
            if (context?.Exception == null)
                return;

            if (context.Exception is HttpStatusException)
                return;

            if (Logger == null)
                Logger = LogManager.GetCurrentClassLogger();

            var requestStream = context.Request.Content.ReadAsStreamAsync().Result;
            requestStream.Position = 0;
            var streamReader = new StreamReader(requestStream, Encoding.UTF8);
            var requestBody = streamReader.ReadToEnd();
            // Truncate large requests to prevent bloating log
            requestBody = new string(requestBody.Take(1000).ToArray());
            Logger.Error(context.Exception, "An error occured: {0}. \n\nRequest details:\nMethod: {1}\nURI: {2}\nBody: {3}",
                context.Exception.Message, context.Request.Method.Method, context.Request.RequestUri.PathAndQuery, requestBody);

            base.Log(context);
        }
    }
}