using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Api.Controllers;
using Portal.Api.Models;
using Portal.BAL.Infrastructure;
using Portal.Domain;
using Portal.Repository.Role;
using Portal.Repository.User;
using Portal.Web.AAA.Authentication;

namespace Portal.WebApi.Tests.Controllers
{
    [TestClass]
    public class SessionControllerTest
    {
        #region Test stuff

        private const string UserNameConst = "john.doe";
        private const string UserIdConst = "guid";
        private const string PasswordConst = "password";
        private const string UrlConst = "http://localhost/";
        private const string HostConst = "localhost";
        private string _appName;
        private Mock<IAuthenticationKeeper> _authenticationKeeperMock;
        private string _password;
        private IPrincipal _principal;
        private string _userName;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IRoleRepository> _roleRepositoryMock;

        private void SetRequiredUser()
        {
            _principal = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserIdConst), null);
        }

        private void RevertUser()
        {
            Thread.CurrentPrincipal = _principal;
        }

        private HttpConfiguration GetHttpConfiguration()
        {
            var configuration = new HttpConfiguration();
            configuration.Formatters.Add(new XmlMediaTypeFormatter());
            configuration.Formatters.Add(new JsonMediaTypeFormatter());
            configuration.Formatters.Add(new JQueryMvcFormUrlEncodedFormatter());
            configuration.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());

            return configuration;
        }

        private IRoleRepository GetFakeRoleRepository()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _roleRepositoryMock.Setup(p => p.GetUserRolesAsync(It.IsAny<string>())).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<List<string>>();
                    tcs.SetResult(new List<string>());
                    return tcs.Task;
                });

            return _roleRepositoryMock.Object;
        }

        private IUserRepository GetFakeUserRepository()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock.Setup(m => m.ValidateFormsUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string, string>((appName, userIdentity, password) =>
                    {
                        string[] parts = userIdentity.Split(':');
                        var contrinuation = new TaskCompletionSource<User>();
                        contrinuation.SetResult(parts[1].Length > 0 ? new User() : null);
                        return contrinuation.Task;
                    })
                .Callback<string, string, string>(
                    (appNameValue, userNameValue, passwordValue) =>
                        {
                            _appName = appNameValue;
                            _userName = userNameValue;
                            _password = passwordValue;
                        }
                );

            _userRepositoryMock.Setup(m => m.GetUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() =>
                    {
                        var continuation = new TaskCompletionSource<User>();
                        continuation.SetResult(new User());
                        return continuation.Task;
                    });

            _userRepositoryMock.Setup(m => m.GetIdentityString(It.IsAny<IdentityProviderType>(), It.IsAny<string>()))
                .Returns<IdentityProviderType, string>((providerName, userId) => string.Format("{0}{1}{2}", providerName.ToString(), ":", userId));

            return _userRepositoryMock.Object;
        }

        private IAuthenticationKeeper GetFakeAuthenticationKeeper()
        {
            _authenticationKeeperMock = new Mock<IAuthenticationKeeper>();
            _authenticationKeeperMock.Setup(m => m.Set(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()));
            _authenticationKeeperMock.Setup(m => m.Get());

            return _authenticationKeeperMock.Object;
        }

        #endregion

        #region Logon method

        [TestMethod]
        public void TestLogonWithCorrectParameters()
        {
            // Arrange
            var requestMock = new Mock<HttpRequestMessage>();
            requestMock.Object.Method = new HttpMethod("POST");
            requestMock.Object.Properties.Add("MS_HttpConfiguration", GetHttpConfiguration());
            requestMock.Object.RequestUri = new Uri(UrlConst);

            SetRequiredUser();

            IUserRepository userRepository = GetFakeUserRepository();
            IAuthenticationKeeper authenticationKeeper = GetFakeAuthenticationKeeper();
            IRoleRepository roleRepository = GetFakeRoleRepository();

            var usersController = new SessionController(userRepository, roleRepository, authenticationKeeper) {Request = requestMock.Object};

            // Act
            Task<HttpResponseMessage> response = usersController.Post(new SessionLoginModel {Email = UserNameConst, Password = PasswordConst})
                .ContinueWith(result => result.Result, TaskContinuationOptions.ExecuteSynchronously);
            response.Wait();

            // Assert
            _userRepositoryMock.Verify(m => m.ValidateFormsUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            _authenticationKeeperMock.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()), Times.Once());

            Assert.IsFalse(response.IsFaulted);
            Assert.IsNotNull(response.Result);
            Assert.IsTrue(response.Result.StatusCode == HttpStatusCode.OK);
            Assert.AreEqual(_userName, "Email:" + UserNameConst);
            Assert.AreEqual(_password, PasswordConst);
            Assert.AreEqual(_appName, HostConst);

            RevertUser();
        }

        [TestMethod]
        public void TestLogonWithInvalidPassword()
        {
            // Arrange
            const string userNameConst = "";

            var requestMock = new Mock<HttpRequestMessage>();
            requestMock.Object.Method = new HttpMethod("POST");
            requestMock.Object.Properties.Add("MS_HttpConfiguration", GetHttpConfiguration());
            requestMock.Object.RequestUri = new Uri(UrlConst);

            SetRequiredUser();

            IUserRepository userRepository = GetFakeUserRepository();
            IAuthenticationKeeper authenticationKeeper = GetFakeAuthenticationKeeper();
            IRoleRepository roleRepository = GetFakeRoleRepository();

            var usersController = new SessionController(userRepository, roleRepository, authenticationKeeper) {Request = requestMock.Object};

            // Act
            Task<HttpResponseMessage> response = usersController.Post(new SessionLoginModel {Email = userNameConst, Password = PasswordConst});
            response.Wait();

            // Assert
            _userRepositoryMock.Verify(m => m.ValidateFormsUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            _authenticationKeeperMock.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()), Times.Never());

            Assert.IsFalse(response.IsFaulted);
            Assert.IsNotNull(response.Result);
            Assert.IsTrue(response.Result.StatusCode == HttpStatusCode.BadRequest);

            RevertUser();
        }

        #endregion

        #region Logoff method

        [TestMethod]
        public void TestLogoffWithCorrectParameters()
        {
            // Arrange

            var requestMock = new Mock<HttpRequestMessage>();
            requestMock.Object.Method = new HttpMethod("POST");
            requestMock.Object.Properties.Add("MS_HttpConfiguration", GetHttpConfiguration());
            requestMock.Object.RequestUri = new Uri(UrlConst);

            SetRequiredUser();

            IUserRepository userRepository = GetFakeUserRepository();
            IAuthenticationKeeper authenticationKeeper = GetFakeAuthenticationKeeper();
            IRoleRepository roleRepository = GetFakeRoleRepository();

            var usersController = new SessionController(userRepository, roleRepository, authenticationKeeper) {Request = requestMock.Object};

            // Act
            Task<HttpResponseMessage> response = usersController.Delete();
            response.Wait();

            // Assert
            _authenticationKeeperMock.Verify(m => m.Clean(), Times.Once());
            Assert.IsFalse(response.IsFaulted);
            Assert.IsNotNull(response.Result);
            Assert.IsTrue(response.Result.StatusCode == HttpStatusCode.OK);
        }

        #endregion
    }
}