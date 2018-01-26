using System;
using System.Linq;
using System.Net.Http;
using NLog;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class LoggerExtensions
    {
        public static void Error(this ILogger logger, Exception exception, HttpRequestMessage httpRequestMessage)
        {
            if (!logger.IsErrorEnabled) return;

            Error(logger, exception, httpRequestMessage.AsFormattedString());
        }

        public static void Error(this ILogger logger, Exception exception, HttpRequestMessage httpRequestMessage, int requestMaxLogLength)
        {
            if (!logger.IsErrorEnabled) return;

            var request = httpRequestMessage.AsFormattedString();

            // Truncate large requests to prevent bloating log
            if (request.Length > requestMaxLogLength)
                request = new string(request.Take(requestMaxLogLength).ToArray()) + "...";

            Error(logger, exception, request);
        }

        private static void Error(this ILogger logger, Exception exception, string request)
        {
            logger.Error(
                exception,
                "Exception Message(s): {0}\n\n\nRequest: {1}",
                string.Join(Environment.NewLine, exception.GetMessages()),
                request
            );
        }

        public static void Warn(this ILogger logger, HttpRequestMessage request, HttpResponseMessage response, string message = "")
        {
            logger.Log(LogLevel.Warn, request, response, message);
        }

        public static void Error(this ILogger logger, HttpRequestMessage request, HttpResponseMessage response, string message = "")
        {
            logger.Log(LogLevel.Error, request, response, message);
        }

        public static void Warn(this ILogger logger, string request, string response, string message = "")
        {
            logger.Log(LogLevel.Warn, request, response, message);
        }

        public static void Error(this ILogger logger, string request, string response, string message = "")
        {
            logger.Log(LogLevel.Error, request, response, message);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, HttpRequestMessage request, HttpResponseMessage response, string message = "")
        {
            if (!logger.IsEnabled(logLevel)) return;

            Log(logger, logLevel, request.AsFormattedString(), response.AsFormattedString(), message);
        }

        private static void Log(this ILogger logger, LogLevel logLevel, string request, string response, string message = "")
        {
            logger.Log(
                logLevel,
                "{0}" +
                "Request: {1}\n\n\n" +
                "Response: {2}\n",
                string.IsNullOrWhiteSpace(message) ? string.Empty : $"{message}\n",
                request,
                response);
        }
    }
}