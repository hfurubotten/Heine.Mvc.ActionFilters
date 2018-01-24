using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Heine.Mvc.ActionFilters.Attributes;
using Heine.Mvc.ActionFilters.Exceptions;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.Attributes
{
    [TestFixture]
    public class ProcessHttpStatusExceptionsAttributeTests
    {
        [TestCase(typeof(ConflictException), HttpStatusCode.Conflict)]
        [TestCase(typeof(NotFoundException), HttpStatusCode.NotFound)]
        [TestCase(typeof(BadRequestException), HttpStatusCode.BadRequest)]
        public void OnException_WhenExceptionIsHttpStatusException_ResultShouldHaveCorrectStatusCode(Type exceptionType, HttpStatusCode expectedStatusCode)
        {
            var exception = Activator.CreateInstance(exceptionType);
            var actionExecutedContext = HttpActionContextFactory.GetActionExecutedContext((HttpStatusException) exception);
            var attribute = new ProcessHttpStatusExceptionsAttribute();
            attribute.OnException(actionExecutedContext);

            actionExecutedContext.ActionContext.Response.StatusCode.Should().Be(expectedStatusCode);
        }

        [TestCase(typeof(ConflictException), "Error message")]
        [TestCase(typeof(NotFoundException), "Error message")]
        [TestCase(typeof(BadRequestException), "Error message")]
        public async Task OnException_WhenExceptionIsHttpStatusException_ResultShouldHaveCorrectMessage(Type exceptionType, string expectedMessage)
        {
            var exception = Activator.CreateInstance(exceptionType, expectedMessage);
            var actionExecutedContext = HttpActionContextFactory.GetActionExecutedContext((HttpStatusException) exception);
            var attribute = new ProcessHttpStatusExceptionsAttribute();
            attribute.OnException(actionExecutedContext);
            var content = await actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<ErrorMessage>();
            content.Message.Should().Be(expectedMessage);
        }

        private class ErrorMessage
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Message { get; set; }
        }
    }
}