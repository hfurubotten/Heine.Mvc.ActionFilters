using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Heine.Mvc.ActionFilters.Tests
{
    [TestClass]
    public class UnitTestActionFilters
    {
        public class ErrorMessage
        {
            public string Message { get; set; }
        }

        [TestMethod]
        public void OnActionExecuted_InvalidModelState_ResultIsBadRequest()
        {
            var actionContext = GetActionContext();
            actionContext.ModelState.AddModelError("key", "error");
            var attribute = new ValidateModelAttribute();
            attribute.OnActionExecuting(actionContext);
            Assert.AreEqual(HttpStatusCode.BadRequest, actionContext.Response.StatusCode);
        }

        [TestMethod]
        public void OnExeption_BadRequestException_ResultIsBadRequest()
        {
            var actionExecutedContext = GetActionExecutedContext(new BadRequestException());
            var attribute = new ProcessBadRequestExceptionAttribute();
            attribute.OnException(actionExecutedContext);
            Assert.AreEqual(HttpStatusCode.BadRequest, actionExecutedContext.ActionContext.Response.StatusCode);
        }

        [TestMethod]
        public void OnExeption_BadRequestException_ResultIsBadRequestWithMessage()
        {
            var actionExecutedContext = GetActionExecutedContext(new BadRequestException("TestMessage"));
            var attribute = new ProcessBadRequestExceptionAttribute();
            attribute.OnException(actionExecutedContext);
            Assert.AreEqual(HttpStatusCode.BadRequest, actionExecutedContext.ActionContext.Response.StatusCode);
            var content = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<ErrorMessage>().Result;
            Assert.AreEqual("TestMessage", content.Message);
        }

        [TestMethod]
        public void OnExeption_NotFoundException_ResultIsNotFound()
        {
            var actionExecutedContext = GetActionExecutedContext(new NotFoundException());
            var attribute = new ProcessNotFoundExceptionAttribute();
            attribute.OnException(actionExecutedContext);
            Assert.AreEqual(HttpStatusCode.NotFound, actionExecutedContext.ActionContext.Response.StatusCode);
        }

        [TestMethod]
        public void OnActionExecuted_ObsoleteMethod_WarningIsLogged()
        {
            var configuration = new LoggingConfiguration();
            var memoryTarget = new MemoryTarget { Name = "mem" };

            configuration.AddTarget(memoryTarget);
            configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, memoryTarget));
            LogManager.Configuration = configuration;

            var actionDescriptor = new Mock<HttpActionDescriptor>();
            actionDescriptor
                .Setup(m => m.GetCustomAttributes<ObsoleteAttribute>())
                .Returns(new Collection<ObsoleteAttribute> { new ObsoleteAttribute() });
            actionDescriptor
                .Setup(m => m.ActionName)
                .Returns("TestAction");

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://www.vg.no/test")
            };

            var controllerContext = new HttpControllerContext
            {
                ControllerDescriptor = new HttpControllerDescriptor
                {
                    ControllerName = "TestController"
                },
                Request = request
            };

            var actionContext = new HttpActionContext
            {
                ControllerContext = controllerContext,
                ActionDescriptor = actionDescriptor.Object
            };

            var attribute = new ReportObsoleteUsageAttribute();
            attribute.OnActionExecuting(actionContext);

            var logs = memoryTarget.Logs;

            Assert.IsTrue(logs[0].Contains("Use of obsolete API method. \n Url: /test \n Controller Name: TestController \n Action Name: TestAction"));

            request.Dispose();
        }

        private static HttpActionContext GetActionContext()
        {
            return new HttpActionContext(new HttpControllerContext { Request = new HttpRequestMessage() }, new ReflectedHttpActionDescriptor());
        }

        private static HttpActionExecutedContext GetActionExecutedContext(Exception exception)
        {
            return new HttpActionExecutedContext
            {
                ActionContext = GetActionContext(),
                Exception = exception
            };
        }
    }
}
