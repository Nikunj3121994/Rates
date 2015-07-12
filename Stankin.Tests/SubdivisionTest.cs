using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StankinQuestionnaire.Areas.Admin.Controllers;
using Moq;
using StankinQuestionnaire.Data.Repository;
using StankinQuestionnaire.Model;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web;
using System.Security.Claims;
using System.Web.Routing;

namespace Stankin.Tests
{
    [TestClass]
    public class SubdivisionTest
    {
        [TestMethod]
        public void UsersViewNotNull()
        {
            // Arrange
            var subdivision = new Mock<ISubdivisionRepository>();
            var user = new Mock<IUserRepository>();
            var mocks = new MockRepository(MockBehavior.Default);
            var context = new Mock<HttpContextBase>();
            var identity = new GenericIdentity("adasd");
            var subID = 1;
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", subID.ToString()));
            var principal = new GenericPrincipal(identity, new[] { "user" });
            context.Setup(c => c.User).Returns(principal);


            user.Setup(u => u.GetForSubdivision(subID)).Returns(new List<ApplicationUser>());
            SubdivisionController controller = new SubdivisionController(subdivision.Object, user.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            // Act
            ViewResult result = controller.Users() as ViewResult;

            //Assert
            Assert.IsNotNull(result.Model);

        }
    }
}
