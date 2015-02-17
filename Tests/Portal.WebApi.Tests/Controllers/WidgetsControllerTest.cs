using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Api.Controllers;
using Portal.Repository.Widget;

namespace Portal.WebApi.Tests.Controllers
{
    [TestClass]
    public class WidgetsControllerTest
    {
        #region Test stuff

        private HttpConfiguration GetHttpConfiguration()
        {
            var configuration = new HttpConfiguration();
            configuration.Formatters.Add(new XmlMediaTypeFormatter());
            configuration.Formatters.Add(new JsonMediaTypeFormatter());
            configuration.Formatters.Add(new JQueryMvcFormUrlEncodedFormatter());
            configuration.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());

            return configuration;
        }

        #endregion

        #region Add method

        [TestMethod]
        public void TestAddCorrectWidgetParameters()
        {
            // Arrange
            const string userNameConst = "tretyakov";
            const string widgetIdConst = "111";
            const string widgetNameConst = "my widget";
            const string widgetContentConst = "<xml /><content></content>";
            const string widgetItemIdConst = "5";

            var requestMock = new Mock<HttpRequestMessage>();
            requestMock.Object.Method = new HttpMethod("POST");
            requestMock.Object.Properties.Add("MS_HttpConfiguration", GetHttpConfiguration());

            string userName = string.Empty,
                   widgetId = string.Empty,
                   widgetName = string.Empty,
                   contentUri = string.Empty;

            IPrincipal principal = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userNameConst), new string[] {});

            var mock = new Mock<IWidgetItemRepository>();
            mock.Setup(m => m.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(widgetItemIdConst)
                .Callback<string, string, string, string>(
                    (userNameValue, widgetIdValue, widgetNameValue, contentUriValue) =>
                        {
                            userName = userNameValue;
                            widgetId = widgetIdValue;
                            widgetName = widgetNameValue;
                            contentUri = contentUriValue;
                        }
                );

            var widgetsController = new WidgetsController(mock.Object) {Request = requestMock.Object};
            string response = string.Empty;
            Exception exception = null;

            // Act
            try
            {
                response = widgetsController.Post(widgetIdConst, widgetNameConst, widgetContentConst);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            mock.Verify(m => m.Add(userNameConst, widgetIdConst, widgetNameConst, It.IsAny<string>()), Times.Once());
            Assert.IsNull(exception);
            Assert.IsTrue(response == widgetItemIdConst);
            Assert.IsTrue(userName == userNameConst);
            Assert.IsTrue(widgetId == widgetIdConst);
            Assert.IsTrue(widgetName == widgetNameConst);
            Assert.IsFalse(string.IsNullOrEmpty(contentUri));
            Assert.IsTrue(File.Exists(contentUri));

            Thread.CurrentPrincipal = principal;
        }

        [TestMethod]
        public void TestAddInvalidWidgetName()
        {
            // Arrange
            const string userNameConst = "tretyakov";
            const string widgetIdConst = "111";
            const string widgetNameConst = "";
            const string widgetContentConst = "<xml /><content></content>";

            var requestMock = new Mock<HttpRequestMessage>();
            requestMock.Object.Method = new HttpMethod("POST");
            requestMock.Object.Properties.Add("MS_HttpConfiguration", GetHttpConfiguration());

            IPrincipal principal = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userNameConst), new string[] {});

            var mock = new Mock<IWidgetItemRepository>();
            mock.Setup(m => m.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("5");

            var widgetsController = new WidgetsController(mock.Object) {Request = requestMock.Object};
            string response = string.Empty;
            Exception exception = null;
            HttpResponseException responseException = null;

            // Act
            try
            {
                response = widgetsController.Post(widgetIdConst, widgetNameConst, widgetContentConst);
            }
            catch (HttpResponseException e)
            {
                responseException = e;
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            mock.Verify(m => m.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            Assert.IsNull(exception);
            Assert.IsTrue(string.IsNullOrEmpty(response));
            Assert.IsNotNull(responseException);
            Assert.IsTrue(responseException.Response.StatusCode == HttpStatusCode.BadRequest);

            Thread.CurrentPrincipal = principal;
        }

        [TestMethod]
        public void TestAddInvalidWidgetId()
        {
            // Arrange
            const string userNameConst = "tretyakov";
            const string widgetIdConst = "";
            const string widgetNameConst = "my widget";
            const string widgetContentConst = "<xml /><content></content>";

            var requestMock = new Mock<HttpRequestMessage>();
            requestMock.Object.Method = new HttpMethod("POST");
            requestMock.Object.Properties.Add("MS_HttpConfiguration", GetHttpConfiguration());

            IPrincipal principal = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userNameConst), new string[] {});

            var mock = new Mock<IWidgetItemRepository>();
            mock.Setup(m => m.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("5");

            var widgetsController = new WidgetsController(mock.Object) {Request = requestMock.Object};
            string response = string.Empty;
            Exception exception = null;
            HttpResponseException responseException = null;

            // Act
            try
            {
                response = widgetsController.Post(widgetIdConst, widgetNameConst, widgetContentConst);
            }
            catch (HttpResponseException e)
            {
                responseException = e;
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            mock.Verify(m => m.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            Assert.IsNull(exception);
            Assert.IsTrue(string.IsNullOrEmpty(response));
            Assert.IsNotNull(responseException);
            Assert.IsTrue(responseException.Response.StatusCode == HttpStatusCode.BadRequest);

            Thread.CurrentPrincipal = principal;
        }

        [TestMethod]
        public void TestAddInvalidWidgetContent()
        {
            // Arrange
            const string userNameConst = "tretyakov";
            const string widgetIdConst = "111";
            const string widgetNameConst = "my widget";
            const string widgetContentConst = "";

            var requestMock = new Mock<HttpRequestMessage>();
            requestMock.Object.Method = new HttpMethod("POST");
            requestMock.Object.Properties.Add("MS_HttpConfiguration", GetHttpConfiguration());

            IPrincipal principal = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userNameConst), new string[] {});

            var mock = new Mock<IWidgetItemRepository>();
            mock.Setup(m => m.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("5");

            var widgetsController = new WidgetsController(mock.Object) {Request = requestMock.Object};
            string response = string.Empty;
            Exception exception = null;
            HttpResponseException responseException = null;

            // Act
            try
            {
                response = widgetsController.Post(widgetIdConst, widgetNameConst, widgetContentConst);
            }
            catch (HttpResponseException e)
            {
                responseException = e;
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            mock.Verify(m => m.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            Assert.IsNull(exception);
            Assert.IsTrue(string.IsNullOrEmpty(response));
            Assert.IsNotNull(responseException);
            Assert.IsTrue(responseException.Response.StatusCode == HttpStatusCode.BadRequest);

            Thread.CurrentPrincipal = principal;
        }

        #endregion
    }
}