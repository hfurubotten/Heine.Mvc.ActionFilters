using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Filters;
using FluentAssertions;
using Heine.Mvc.ActionFilters.ActionFilterAttributes;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.ActionFilterAttributes 
{
    [TestFixture]
    public class ResponseCorrelationAttributeTests
    {
        [Test]
        public void ResponseCorrelationAttribute_OnException_HeaderValueShouldBeSetOnResponse()
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

            var attribute = new ResponseCorrelationAttribute(headerKey);

            // Act
            attribute.ExecuteExceptionFilterAsync(actionContext, CancellationToken.None);

            // Assert
            actionContext.Request.Headers.Contains(headerKey).Should().BeTrue();
            actionContext.Request.Headers.GetValues(headerKey).Single().Should().Be(headerValue);
        }
    }
}