using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.Api.Commands.Profile;
using Portal.Api.Models;
using Portal.Domain;
using Portal.Repository.User;
using Portal.Web.AAA.Authentication;
using Profile = Portal.Interface.Profile;

namespace Portal.WebApi.Tests.Controllers
{
    [TestClass]
    public class ProfileControllerTests
    {
        #region Test Stuff

        private const string UserIdConst = "guid";
        private const string UserNameConst = "Vasiliy Pupkin";
        private const string UserEmailConst = "vasya@gmail.com";
        private const string UserPassConst = "password";
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IAuthenticationKeeper> _authenticationKeeperMock;

        private ApiController GetFakeApiController(bool isMultipart = false)
        {
            var controllerMock = new Mock<ApiController>();

            var requestMock = new Mock<HttpRequestMessage>();
            if (isMultipart)
            {
                HttpContent httpContent = new StringContent(string.Empty);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                httpContent.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", "----WebKitFormBoundaryAIv1HQdnxqjt9a5y"));
                httpContent.Headers.ContentLength = 465;

                requestMock.Object.Content = httpContent;
            }

            requestMock.Object.RequestUri = new Uri("http://clickberry.tv");

            requestMock.Object.Properties.Add("MS_HttpConfiguration", GetHttpConfiguration());
            controllerMock.Object.Request = requestMock.Object;

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserIdConst), null);

            return controllerMock.Object;
        }

        private IUserRepository GetFakeUserRepository()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock.Setup(m => m.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>())).Returns(() =>
            {
                var tcs = new TaskCompletionSource<User>();
                tcs.SetResult(new User());
                return tcs.Task;
            });
            _userRepositoryMock.Setup(m => m.GetUserProfileAsync(It.IsAny<string>())).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<Profile>();
                    tcs.SetResult(new Profile());
                    return tcs.Task;
                });
            _userRepositoryMock.Setup(m => m.EditUserProfileAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() =>
                    {
                        var tcs = new TaskCompletionSource<HttpResponseMessage>();
                        tcs.SetResult(new HttpResponseMessage(HttpStatusCode.OK));
                        return tcs.Task;
                    });
            _userRepositoryMock.Setup(m => m.DeleteUserAsync(It.IsAny<string>())).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<int>();
                    tcs.SetResult(0);
                    return tcs.Task;
                });

            return _userRepositoryMock.Object;
        }

        private IAuthenticationKeeper GetFakeAuthenticationKeeper()
        {
            _authenticationKeeperMock = new Mock<IAuthenticationKeeper>();
            _authenticationKeeperMock.Setup(m => m.Set(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<bool>()));
            _authenticationKeeperMock.Setup(m => m.Get());

            return _authenticationKeeperMock.Object;
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

        #endregion

        #region Add Profile

        [TestMethod]
        public void AddUserProfileTest()
        {
            var userProfile = new ProfilePostModel
                {
                    UserName = UserNameConst,
                    Email = UserEmailConst,
                    Password = UserPassConst,
                    ConfirmPassword = UserPassConst
                };

            var command = new AddCommand(GetFakeApiController(), GetFakeUserRepository(), GetFakeAuthenticationKeeper(), userProfile);
            Task<HttpResponseMessage> result = command.Execute();
            result.Wait();

            Assert.IsFalse(result.IsFaulted);
            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(HttpResponseMessage));
            Assert.AreEqual(result.Result.StatusCode, HttpStatusCode.Created);
        }

        #endregion

        #region Get Profile

        [TestMethod]
        public void GetUserProfileTest()
        {
            var command = new GetCommand(GetFakeApiController(), GetFakeUserRepository());
            Task<Profile> result = command.Execute();
            result.Wait();

            Assert.IsFalse(result.IsFaulted);
            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof (Profile));
        }

        #endregion

        #region Edit Profile

        [TestMethod]
        public void EditUserProfileTest()
        {
            var profilePutModel = new ProfilePutModel();
            var command = new EditCommand(GetFakeApiController(), GetFakeUserRepository(), profilePutModel);
            Task<HttpResponseMessage> result = command.Execute();
            result.Wait();

            Assert.IsFalse(result.IsFaulted);
            Assert.IsNotNull(result.Result);
            Assert.IsTrue(result.Result.StatusCode == HttpStatusCode.OK);
        }

        #endregion

        #region Delete Profile

        [TestMethod]
        public void DeleteUserProfileTest()
        {
            var command = new DeleteCommand(GetFakeApiController(), GetFakeUserRepository(), GetFakeAuthenticationKeeper());
            Task<HttpResponseMessage> result = command.Execute();
            result.Wait();

            Assert.IsFalse(result.IsFaulted);
            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(HttpResponseMessage));
            Assert.AreEqual(result.Result.StatusCode, HttpStatusCode.OK);
        }

        #endregion
    }
}