using Moq;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests
{
    [TestFixture]
    public class UnitTestActionFilters
    {
        public class ErrorMessage
        {
            public string Message { get; set; }
        }

        [Test]
        public void OnActionExecuted_InvalidModelState_ResultIsBadRequest()
        {
            var actionContext = GetActionContext();
            actionContext.ModelState.AddModelError("key", "error");
            var attribute = new ValidateModelAttribute();
            attribute.OnActionExecuting(actionContext);
            Assert.AreEqual(HttpStatusCode.BadRequest, actionContext.Response.StatusCode);
        }

        [TestCase(typeof(ConflictException), HttpStatusCode.Conflict)]
        [TestCase(typeof(NotFoundException), HttpStatusCode.NotFound)]
        [TestCase(typeof(BadRequestException), HttpStatusCode.BadRequest)]
        public void OnException_WhenExceptionIsHttpStatusException_ResultShouldHaveCorrectStatusCode(Type exceptionType, HttpStatusCode expectedStatusCode)
        {
            var exception = Activator.CreateInstance(exceptionType);
            var actionExecutedContext = GetActionExecutedContext((HttpStatusException)exception);
            var attribute = new ProcessHttpStatusExceptionsAttribute();
            attribute.OnException(actionExecutedContext);
            Assert.AreEqual(expectedStatusCode, actionExecutedContext.ActionContext.Response.StatusCode);
        }

        [TestCase(typeof(ConflictException), "Error message")]
        [TestCase(typeof(NotFoundException), "Error message")]
        [TestCase(typeof(BadRequestException), "Error message")]
        public async Task OnException_WhenExceptionIsHttpStatusException_ResultShouldHaveCorrectMessage(Type exceptionType, string expectedMessage)
        {
            var exception = Activator.CreateInstance(exceptionType, expectedMessage);
            var actionExecutedContext = GetActionExecutedContext((HttpStatusException)exception);
            var attribute = new ProcessHttpStatusExceptionsAttribute();
            attribute.OnException(actionExecutedContext);
            var content = await actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<ErrorMessage>();
            Assert.AreEqual(expectedMessage, content.Message);
        }

        [Test]
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
