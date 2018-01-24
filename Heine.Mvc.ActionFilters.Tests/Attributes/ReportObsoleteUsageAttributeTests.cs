using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Web.Http.Controllers;
using FluentAssertions;
using Heine.Mvc.ActionFilters.Attributes;
using Moq;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.Attributes
{
    [TestFixture]
    public class ReportObsoleteUsageAttributeTests
    {
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

            logs[0].Should().Contain("Use of obsolete API method.");

            request.Dispose();
        }
    }
}