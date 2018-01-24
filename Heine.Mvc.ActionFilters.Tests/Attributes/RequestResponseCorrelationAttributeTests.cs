using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FluentAssertions;
using Heine.Mvc.ActionFilters.Attributes;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Heine.Mvc.ActionFilters.Tests.Attributes
{
    [TestFixture]
    public class RequestResponseCorrelationAttributeTests
    {
        [Test]
        public void RequestResponseCorrelationAttributeWithFunc_WhenHeaderDoesNotContainKey_HeaderValueShouldBeAdded()
        {
            // Arrange
            var actionContext = HttpActionContextFactory.GetActionContext();
            const string headerKey = "X-CorrelationID";
            var headerValue = Guid.NewGuid().ToString();
            var attribute = new RequestResponseCorrelationAttribute(headerKey, () => headerValue);

            // Act
            attribute.OnActionExecuting(actionContext);

            // Assert
            actionContext.Request.Headers.Contains(headerKey).Should().BeTrue();
            actionContext.Request.Headers.GetValues(headerKey).Single().Should().Be(headerValue);
        }

        [Test]
        public void RequestResponseCorrelationAttributeWithConstant_WhenHeaderDoesNotContainKey_HeaderValueShouldBeAdded()
        {
            // Arrange
            var actionContext = HttpActionContextFactory.GetActionContext();
            const string headerKey = "X-CorrelationID";
            var headerValue = Guid.NewGuid().ToString();
            var attribute = new RequestResponseCorrelationAttribute(headerKey, headerValue);

            // Act
            attribute.OnActionExecuting(actionContext);

            // Assert
            actionContext.Request.Headers.Contains(headerKey).Should().BeTrue();
            actionContext.Request.Headers.GetValues(headerKey).Single().Should().Be(headerValue);
        }

        [Test]
        public void RequestResponseCorrelationAttribute_WhenHeaderDoesContainKey_HeaderValueShouldNotBeOverwritten()
        {
            // Arrange
            const string headerKey = "X-CorrelationID";
            const string headerValue = "TEST";
            var request = new HttpRequestMessage();
            request.Headers.Add(headerKey, new List<string>{ headerValue });
            var actionContext = HttpActionContextFactory.GetActionContext(request);
            var attribute = new RequestResponseCorrelationAttribute(headerKey, () => Guid.NewGuid().ToString());

            // Act
            attribute.OnActionExecuting(actionContext);

            // Assert
            actionContext.Request.Headers.Contains(headerKey).Should().BeTrue();
            actionContext.Request.Headers.GetValues(headerKey).Single().Should().Be(headerValue);
        }

        [Test]
        public void RequestResponseCorrelationAttribute_OnException_HeaderValueShouldBeSetOnResponse()
        {
            // Arrange
            const string headerKey = "X-CorrelationID";
            var headerValue = Guid.NewGuid().ToString();

            var request = new HttpRequestMessage();
            request.Headers.Add(headerKey, new List<string> { headerValue });
            var actionContext = new HttpActionExecutedContext
            {
                ActionContext = HttpActionContextFactory.GetActionContext(request, new HttpResponseMessage()),
                Exception = null
            };

            var attribute = new RequestResponseCorrelationAttribute(headerKey, () => headerValue);

            // Act
            attribute.ExecuteExceptionFilterAsync(actionContext, CancellationToken.None);

            // Assert
            actionContext.Request.Headers.Contains(headerKey).Should().BeTrue();
            actionContext.Request.Headers.GetValues(headerKey).Single().Should().Be(headerValue);
        }
    }
}