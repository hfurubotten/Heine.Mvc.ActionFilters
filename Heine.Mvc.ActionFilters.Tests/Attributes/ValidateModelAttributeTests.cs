using System.Net;
using FluentAssertions;
using Heine.Mvc.ActionFilters.Attributes;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.Attributes
{
    [TestFixture]
    public class ValidateModelAttributeTests
    {
        [Test]
        public void OnActionExecuted_InvalidModelState_ResultIsBadRequest()
        {
            var actionContext = HttpActionContextFactory.GetActionContext();
            actionContext.ModelState.AddModelError("key", "error");
            var attribute = new ValidateModelAttribute();
            attribute.OnActionExecuting(actionContext);

            actionContext.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}