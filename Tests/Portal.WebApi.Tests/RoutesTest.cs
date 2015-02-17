using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.WebApi.Tests.Infrastructure;

namespace Portal.WebApi.Tests
{
    [TestClass]
    public class RoutesTest
    {
        [TestMethod]
        public void TestWebApiProjectRoutes()
        {
            // check for the URL that we hope to receive
            RouteTest.TestRouteMatch("~/projects", "Projects", "Post", null, "POST");
            RouteTest.TestRouteMatch("~/projects", "Projects", "Get");
            RouteTest.TestRouteMatch("~/projects/222", "Projects", "Get", new {id = "222"});
            RouteTest.TestRouteMatch("~/projects/222", "Projects", "Put", new {id = "222"}, "PUT");
            RouteTest.TestRouteMatch("~/projects/222", "Projects", "Delete", new {id = "222"}, "DELETE");
        }

        [TestMethod]
        public void TestWebApiProjectWrongRoutes()
        {
            // ensure that too many or too few segments fails to match
            RouteTest.TestRouteFail("~/projects/add/get/");
        }

        [TestMethod]
        public void TestWebApiAccountSessionRoutes()
        {
            // check for the URL that we hope to receive
            RouteTest.TestRouteMatch("~/accounts/session", "Session", "Post", null, "POST");
            RouteTest.TestRouteMatch("~/accounts/session", "Session", "Get");
        }

        [TestMethod]
        public void TestWebApiAccountProfileRoutes()
        {
            // check for the URL that we hope to receive
            RouteTest.TestRouteMatch("~/accounts/profile", "Profile", "Post", null, "POST");
            RouteTest.TestRouteMatch("~/accounts/profile", "Profile", "Get");
            RouteTest.TestRouteMatch("~/accounts/profile", "Profile", "Put", null, "PUT");
            RouteTest.TestRouteMatch("~/accounts/profile", "Profile", "Delete", null, "DELETE");
        }
    }
}