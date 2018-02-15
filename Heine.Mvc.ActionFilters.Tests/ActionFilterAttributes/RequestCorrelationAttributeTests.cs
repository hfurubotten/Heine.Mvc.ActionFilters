using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Heine.Mvc.ActionFilters.ActionFilterAttributes;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.ActionFilterAttributes
{
    [TestFixture]
    public class RequestCorrelationAttributeTests
    {
        [Test]
        public void RequestCorrelationAttributeWithFunc_WhenHeaderDoesNotContainKey_HeaderValueShouldBeAdded()
        {
            // Arrange
            var actionContext = HttpActionContextFactory.GetActionContext();
            const string headerKey = "X-CorrelationID";
            var headerValue = Guid.NewGuid().ToString();
            var attribute = new RequestCorrelationAttribute(headerKey, () => headerValue);

            // Act
            attribute.OnActionExecuting(actionContext);

            // Assert
            actionContext.Request.Headers.Contains(headerKey).Should().BeTrue();
            actionContext.Request.Headers.GetValues(headerKey).Single().Should().Be(headerValue);
        }

        [Test]
        public void RequestCorrelationAttributeWithConstant_WhenHeaderDoesNotContainKey_HeaderValueShouldBeAdded()
        {
            // Arrange
            var actionContext = HttpActionContextFactory.GetActionContext();
            const string headerKey = "X-CorrelationID";
            var headerValue = Guid.NewGuid().ToString();
            var attribute = new RequestCorrelationAttribute(headerKey, headerValue);

            // Act
            attribute.OnActionExecuting(actionContext);

            // Assert
            actionContext.Request.Headers.Contains(headerKey).Should().BeTrue();
            actionContext.Request.Headers.GetValues(headerKey).Single().Should().Be(headerValue);
        }

        [Test]
        public void RequestCorrelationAttribute_WhenHeaderDoesContainKey_HeaderValueShouldNotBeOverwritten()
        {
            // Arrange
            const string headerKey = "X-CorrelationID";
            const string headerValue = "TEST";
            var request = new HttpRequestMessage();
            request.Headers.Add(headerKey, new List<string>{ headerValue });
            var actionContext = HttpActionContextFactory.GetActionContext(request);
            var attribute = new RequestCorrelationAttribute(headerKey, () => Guid.NewGuid().ToString());

            // Act
            attribute.OnActionExecuting(actionContext);

            // Assert
            actionContext.Request.Headers.Contains(headerKey).Should().BeTrue();
            actionContext.Request.Headers.GetValues(headerKey).Single().Should().Be(headerValue);
        }
    }
}