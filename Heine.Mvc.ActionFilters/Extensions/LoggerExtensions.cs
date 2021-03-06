﻿using System;
using System.Diagnostics;
using System.Net.Http;
using NLog;

namespace Heine.Mvc.ActionFilters.Extensions
{
    public static class HttpRequestResponseLoggerExtensions
    {
        public static void Debug(this ILogger logger, HttpRequestMessage request, HttpResponseMessage response, string message = "")
        {
            logger.Log(LogLevel.Debug, request, response, message);
        }
        public static void Info(this ILogger logger, HttpRequestMessage request, HttpResponseMessage response, string message = "")
        {
            logger.Log(LogLevel.Info, request, response, message);
        }
        public static void Warn(this ILogger logger, HttpRequestMessage request, HttpResponseMessage response, string message = "")
        {
            logger.Log(LogLevel.Warn, request, response, message);
        }

        public static void Error(this ILogger logger, HttpRequestMessage request, HttpResponseMessage response, string message = "")
        {
            logger.Log(LogLevel.Error, request, response, message);
        }


        public static void Debug(this ILogger logger, Stopwatch stopwatch, HttpRequestMessage request, HttpResponseMessage response, string message = "")
        {
            logger.Log(LogLevel.Debug, request, response, message, stopwatch);
        }
        public static void Info(this ILogger logger, Stopwatch stopwatch, HttpRequestMessage request, HttpResponseMessage response, string message = "")
        {
            logger.Log(LogLevel.Info, request, response, message, stopwatch);
        }
        public static void Warn(this ILogger logger, Stopwatch stopwatch, HttpRequestMessage request, HttpResponseMessage response, string message = "")
        {
            logger.Log(LogLevel.Warn, request, response, message, stopwatch);
        }

        public static void Error(this ILogger logger, Stopwatch stopwatch, HttpRequestMessage request, HttpResponseMessage response, string message = "")
        {
            logger.Log(LogLevel.Error, request, response, message, stopwatch);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, HttpRequestMessage request, HttpResponseMessage response, string message = "", Stopwatch stopwatch = null)
        {
            if (!logger.IsEnabled(logLevel)) return;

            // ReSharper disable InconsistentNaming
            var _request = request.Destruct();
            var _response = response.Destruct();
            // ReSharper restore InconsistentNaming

            logger.Log(logLevel, message + 
                "Request: \n" +
                "Method: {HttpMethod}, \n" +
                "RequestUri: {RequestUri}, \n" +
                "Version: {HttpRequestVersion}, \n" +
                "Content Class Name: {HttpRequestContentClassName}, \n" +
                "Headers: {@HttpRequestHeaders} \n\n" +
                "Body: \n{HttpRequestBody} \n" +
                "\n\n" +
                "Response: \n" +
                "HttpStatusCode: {HttpStatusCode} \n" +
                "Status Reason: {HttpStatusReason} \n" +
                "Http version: {HttpResponseVersion} \n" +
                "Content Class Name: {HttpResponseContentClassName} \n" +
                "Headers: {@HttpResponseHeaders} \n\n" +
                "Body: \n{HttpResponseBody} \n\n" +
                "Benchmarking: \n" +
                "ElapsedTime: {ElapsedTime}", 

                _request.method,
                _request.requestUri,
                _request.version,
                _request.contentTypeName,
                _request.headers,
                _request.body, 

                _response.statusCode,
                _response.reasonPhrase,
                _response.version,
                _response.contentTypeName,
                _response.headers,
                _response.body,
                
                stopwatch?.ElapsedMilliseconds);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, Exception ex, HttpRequestMessage request, HttpResponseMessage response, string message = "", Stopwatch stopwatch = null)
        {
            if (!logger.IsEnabled(logLevel)) return;

            // ReSharper disable InconsistentNaming
            var _request = request.Destruct();
            var _response = response.Destruct();
            // ReSharper restore InconsistentNaming

            logger.Log(logLevel, ex, message + 
                "Request: \n" +
                "Method: {HttpMethod}, \n" +
                "RequestUri: {RequestUri}, \n" +
                "Version: {HttpRequestVersion}, \n" +
                "Content Class Name: {HttpRequestContentClassName}, \n" +
                "Headers: {@HttpRequestHeaders} \n\n" +
                "Body: \n{HttpRequestBody} \n" +
                "\n\n" +
                "Response: \n" +
                "HttpStatusCode: {HttpStatusCode} \n" +
                "Status Reason: {HttpStatusReason} \n" +
                "Http version: {HttpResponseVersion} \n" +
                "Content Class Name: {HttpResponseContentClassName} \n" +
                "Headers: {@HttpResponseHeaders} \n\n" +
                "Body: \n{HttpResponseBody} \n\n", 
                "Benchmarking: \n" +
                "ElapsedTime: {ElapsedTime}", 

                _request.method,
                _request.requestUri,
                _request.version,
                _request.contentTypeName,
                _request.headers,
                _request.body, 

                _response.statusCode,
                _response.reasonPhrase,
                _response.version,
                _response.contentTypeName,
                _response.headers,
                _response.body,
                
                stopwatch?.ElapsedMilliseconds);
        }
    }

    public static class HttpResponseLoggerExtensions
    {
        public static void Error(this ILogger logger, HttpResponseMessage response)
        {
            Log(logger, LogLevel.Error, response);
        }

        public static void Warn(this ILogger logger, HttpResponseMessage response)
        {
            Log(logger, LogLevel.Warn, response);
        }

        public static void Info(this ILogger logger, HttpResponseMessage response)
        {
            Log(logger, LogLevel.Info, response);
        }

        public static void Debug(this ILogger logger, HttpResponseMessage response)
        {
            Log(logger, LogLevel.Debug, response);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, HttpResponseMessage response, string message = "")
        {
            if (!logger.IsEnabled(logLevel)) return;

            // ReSharper disable once InconsistentNaming
            var _response = response.Destruct();

            logger.Log(logLevel, message + 
                "Response: \n" +
                "HttpStatusCode: {HttpStatusCode} \n" +
                "Status Reason: {HttpStatusReason} \n" +
                "Http version: {HttpResponseVersion} \n" +
                "Content Class Name: {HttpResponseContentClassName} \n" +
                "Headers: {@HttpResponseHeaders} \n\n" +
                "Body: \n{HttpResponseBody}", 
                _response.statusCode,
                _response.reasonPhrase,
                _response.version,
                _response.contentTypeName,
                _response.headers,
                _response.body);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, Exception ex, HttpResponseMessage response, string message = "")
        {
            if (!logger.IsEnabled(logLevel)) return;

            // ReSharper disable once InconsistentNaming
            var _response = response.Destruct();

            logger.Log(logLevel, ex, message + 
                "Response: \n" +
                "HttpStatusCode: {HttpStatusCode} \n" +
                "Status Reason: {HttpStatusReason} \n" +
                "Http version: {HttpResponseVersion} \n" +
                "Content Class Name: {HttpResponseContentClassName} \n" +
                "Headers: {@HttpResponseHeaders} \n\n" +
                "Body: \n{HttpResponseBody}", 
                _response.statusCode,
                _response.reasonPhrase,
                _response.version,
                _response.contentTypeName,
                _response.headers,
                _response.body);
        }
    }

    public static class HttpRequestLoggerExtensions
    {
        public static void Error(this ILogger logger, Exception exception, HttpRequestMessage httpRequestMessage)
        {
            Log(logger, LogLevel.Error, exception, httpRequestMessage);
        }

        public static void Error(this ILogger logger, Exception exception, HttpRequestMessage httpRequestMessage, int requestMaxLogLength)
        {
            Log(logger, LogLevel.Error, exception, httpRequestMessage);
        }

        public static void Error(this ILogger logger, HttpRequestMessage request)
        {
            Log(logger, LogLevel.Error, request);
        }

        public static void Warn(this ILogger logger, HttpRequestMessage request)
        {
            Log(logger, LogLevel.Warn, request);
        }

        public static void Info(this ILogger logger, HttpRequestMessage request)
        {
            Log(logger, LogLevel.Info, request);
        }

        public static void Debug(this ILogger logger, HttpRequestMessage request)
        {
            Log(logger, LogLevel.Debug, request);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, HttpRequestMessage request, string message = "")
        {
            if (!logger.IsEnabled(logLevel)) return;

            // ReSharper disable once InconsistentNaming
            var _request = request.Destruct();

            logger.Log(logLevel, message + 
                "Request: \n" +
                "Method: {HttpMethod}, \n" +
                "RequestUri: {RequestUri}, \n" +
                "Version: {HttpRequestVersion}, \n" +
                "Content Class Name: {HttpRequestContentClassName}, \n" +
                "Headers: {@HttpRequestHeaders} \n\n" +
                "Body: \n{HttpRequestBody}", 
                _request.method,
                _request.requestUri,
                _request.version,
                _request.contentTypeName,
                _request.headers,
                _request.body);
        }

        public static void Log(this ILogger logger, LogLevel logLevel, Exception ex, HttpRequestMessage request, string message = "")
        {
            if (!logger.IsEnabled(logLevel)) return;

            // ReSharper disable once InconsistentNaming
            var _request = request.Destruct();

            logger.Log(logLevel, ex, message + 
                "Request: \n" +
                "Method: {HttpMethod}, \n" +
                "RequestUri: {RequestUri}, \n" +
                "Version: {HttpRequestVersion}, \n" +
                "Content Class Name: {HttpRequestContentClassName}, \n" +
                "Headers: {@HttpRequestHeaders} \n\n" +
                "Body: \n{HttpRequestBody}", 
                _request.method,
                _request.requestUri,
                _request.version,
                _request.contentTypeName,
                _request.headers,
                _request.body);
        }
    }
}