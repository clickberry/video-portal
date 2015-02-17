using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Web.Tests.Infrastructure;

namespace Portal.Web.Tests
{
    [TestClass]
    public class WebRoutesTest
    {
        [TestMethod]
        public void TestHomeControllerRoutes()
        {
            // check for the URL that we hope to receive
            RouteTest.TestRouteMatch("~/", "Home", "Index");
            RouteTest.TestRouteMatch("~/Home/Index", "Home", "Index");
            RouteTest.TestRouteMatch("~/Agreement", "Home", "Agreement");
        }

        [TestMethod]
        public void TestHomeControllerWrongRoutes()
        {
            // ensure that too many or too few segments fails to match
            RouteTest.TestRouteFail("~/Home/Index/Segment");
        }

        //[TestMethod]
        //public void TestAccountControllerRoutes()
        //{
        //    // check for the URL that we hope to receive
        //    HttpRoute.TestRouteMatch("~/Account/Register", "Account", "Register");
        //    HttpRoute.TestRouteMatch("~/Account/Logon", "Account", "Logon");
        //    HttpRoute.TestRouteMatch("~/Account/Logoff", "Account", "Logoff");
        //    HttpRoute.TestRouteMatch("~/Account/ChangePassword", "Account", "ChangePassword");
        //    HttpRoute.TestRouteMatch("~/Account/Edit", "Account", "Edit");
        //    HttpRoute.TestRouteMatch("~/Account/Restore", "Account", "Restore");

        //    // ensure that too many or too few segments fails to match
        //    HttpRoute.TestRouteFail("~/Account");
        //}

        //[TestMethod]
        //public void TestVideoControllerRoutes()
        //{
        //    // check for the URL that we hope to receive
        //    HttpRoute.TestRouteMatch("~/Video", "Video", "List");
        //    HttpRoute.TestRouteMatch("~/Video/List", "Video", "List");
        //    HttpRoute.TestRouteMatch("~/Video/Edit/1", "Video", "Edit", new {id = "1"});
        //    HttpRoute.TestRouteMatch("~/Video/Delete/1", "Video", "Delete", new {id = "1"});

        //    // ensure that too many or too few segments fails to match
        //    HttpRoute.TestRouteFail("~/Video/Edit");
        //    HttpRoute.TestRouteFail("~/Video/Delete");
        //}

        //[TestMethod]
        //public void TestStatisticsControllerRoutes()
        //{
        //    // check for the URL that we hope to receive
        //    HttpRoute.TestRouteMatch("~/Statistics", "Statistics", "List");
        //    HttpRoute.TestRouteMatch("~/Statistics/List", "Statistics", "List");

        //    // ensure that too many or too few segments fails to match
        //}

        
    }
}
