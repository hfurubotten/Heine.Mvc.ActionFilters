using System.IO;
using System.Security.Principal;
using System.Web;
using Heine.Mvc.ActionFilters.Attributes;
using Heine.Mvc.ActionFilters.Interfaces;
using Moq;
using NUnit.Framework;

namespace Heine.Mvc.ActionFilters.Tests.Attributes
{
    [TestFixture]
    public class AuditRequestAttributeTests
    {
        [Test]
        public void OnActionExecuted_WithAuditRequestAndAuthenticatedUser_ShouldCallAuditI()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://vg.no", ""),
                new HttpResponse(new StringWriter())
            );

            // User is logged in
            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity("username"),
                new string[0]
            );

            var actionContext = HttpActionContextFactory.GetActionContext();
            var auditRequest = new Mock<IAuditable>();
            actionContext.ActionArguments.Add("request", auditRequest.Object);
            var attribute = new AuditRequestAttribute();
            attribute.OnActionExecuting(actionContext);
            auditRequest.Verify(m => m.Audit(It.Is<string>(s => s.Equals("username"))), Times.Exactly(1));
        }

        [Test]
        public void OnActionExecuted_WithAuditRequestAndAnonymousUser_ShouldNotCallAuditI()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://vg.no", ""),
                new HttpResponse(new StringWriter())
            );

            // User is not logged in
            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity(string.Empty),
                new string[0]
            );

            var actionContext = HttpActionContextFactory.GetActionContext();
            var auditRequest = new Mock<IAuditable>();
            actionContext.ActionArguments.Add("request", auditRequest.Object);
            var attribute = new AuditRequestAttribute();
            attribute.OnActionExecuting(actionContext);
            auditRequest.Verify(m => m.Audit(It.IsAny<string>()), Times.Never());
        }
    }
}