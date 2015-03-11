// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Services;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Asp.Infrastructure.Attributes;
using Asp.Infrastructure.Attributes.Mvc;
using Configuration;
using Newtonsoft.Json;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.BLL.Subscriptions;
using Portal.DAL.Infrastructure.Authentication;
using Portal.Domain.PortalContext;
using Portal.Domain.ProfileContext;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.User;
using Portal.Exceptions.CRUD;
using Portal.Web.Constants;
using Portal.Web.Models;

namespace Portal.Web.Controllers
{
    /// <summary>
    ///     Handles user account actions. Hell, yeah!
    /// </summary>
    [RoutePrefix("account")]
    public class AccountController : ControllerBase
    {
        private const string ReturnUrl = "returnUrl";
        private readonly IAuthenticationService _authenticationService;
        private readonly ICompanyService _companyService;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly ISocialNetworkNotificationFactory _notificationFactory;
        private readonly IPasswordService _passwordService;
        private readonly IPendingClientService _pendingClientService;
        private readonly IProductIdExtractor _productIdExtractor;
        private readonly ITokenDataExtractorFactory _tokenDataExtractorFactory;
        private readonly IUserService _userService;

        public AccountController(
            IAuthenticationService authenticationService,
            IUserService userService,
            IEmailNotificationService emailNotificationService,
            IPortalFrontendSettings settings,
            ITokenDataExtractorFactory tokenDataExtractorFactory,
            ISocialNetworkNotificationFactory notificationFactory,
            IProductIdExtractor productIdExtractor,
            IPendingClientService pendingClientService,
            IPasswordService passwordService,
            ICompanyService companyService)
            : base(settings)
        {
            _emailNotificationService = emailNotificationService;
            _tokenDataExtractorFactory = tokenDataExtractorFactory;
            _notificationFactory = notificationFactory;
            _productIdExtractor = productIdExtractor;
            _pendingClientService = pendingClientService;
            _passwordService = passwordService;
            _companyService = companyService;
            _authenticationService = authenticationService;
            _userService = userService;
        }


        //
        // GET: /account/postlogin

        [Route("postlogin")]
        public ActionResult PostLogin()
        {
            return RedirectToAction("Index", "Home");
        }


        //
        // POST: /account/iplogin

        [Route("iplogin")]
        [HttpPost]
        [StatUserRegistrationMvc]
        [StatUserLoginMvc]
        public async Task<ActionResult> IpLogin(UserModel model)
        {
            TokenData tokenData;

            // Try to get ws federation claims
            SocialIdentity socialIdentity;
            ClaimsIdentity claimsIdentity;
            var claims = new Dictionary<string, string>();
            if ((socialIdentity = User.Identity as SocialIdentity) != null && socialIdentity.IsAuthenticated)
            {
                // Ws federation authentication for already signed in user
                claims = socialIdentity.SocialClaims.ToDictionary(claim => claim.Type, claim => claim.Value);
            }
            else if ((claimsIdentity = User.Identity as ClaimsIdentity) != null && claimsIdentity.IsAuthenticated)
            {
                // Ws federation authentication
                claims = claimsIdentity.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
            }

            // Try to extract token data from claims
            if (claims.Count > 0)
            {
                ITokenDataExtractor tokenDataExtractor = _tokenDataExtractorFactory.CreateTokenDataExtractor(TokenType.Claims);
                tokenData = tokenDataExtractor.Get(new IpData { Token = JsonConvert.SerializeObject(claims) });

                FederatedAuthentication.WSFederationAuthenticationModule.SignOut(true);
            }
            else if (model != null && !string.IsNullOrEmpty(model.UserIdentifier))
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Registration form data
                tokenData = model;
            }
            else
            {
                // Failure while authentication
                return View("_IpAccessDeclined");
            }

            DomainUser user = null;
            DomainUser currentUser = null;
            user = await GetUserByToken(tokenData);
            currentUser = await GetCurrentUser();

            try
            {
                if (currentUser != null)
                {
                    if (user == null)
                    {
                        await _userService.AddMembersipAsync(currentUser.Id, tokenData);
                        user = await GetUserByEmail(tokenData);

                        if (user != null)
                        {
                            if (user.Id != currentUser.Id)
                            {
                                await _userService.MergeFromAsync(user.Id, currentUser.Id);
                            }
                        }
                    }

                    // Set cookies
                    await _authenticationService.SetUserAsync(currentUser, tokenData);
                }
                else
                {
                    if (user == null)
                    {
                        user = await GetUserByEmail(tokenData);
                        if (user != null)
                        {
                            await _userService.AddMembersipAsync(user.Id, tokenData);
                        }
                        else
                        {
                            user = await CompleteRegistrationAsync(tokenData);
                        }
                    }

                    // Set cookies if not authenticated
                    await _authenticationService.SetUserAsync(user, tokenData);
                }
            }
            catch (ForbiddenException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed IpLogin '{0}:{1}': {2}", tokenData.IdentityProvider, tokenData.UserIdentifier, e);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            // For statistics
            HttpContext.Items.Add("isLogin", true);

            // Return user to the authentication page
            if (Request.Cookies.AllKeys.Contains(ReturnUrl))
            {
                string returnUrl = HttpUtility.UrlDecode(Request.Cookies[ReturnUrl].Value);

                // Remove cookie
                Response.Cookies.Add(new HttpCookie(ReturnUrl) { Expires = DateTime.Now.AddDays(-1d) });

                // Redirect to requested url
                if (!string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith("/"))
                {
                    return Redirect(returnUrl);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task<DomainUser> GetUserByToken(TokenData tokenData)
        {
            DomainUser user = null;
            try
            {
                user = await _userService.FindByIdentityAsync(tokenData.IdentityProvider, tokenData.UserIdentifier);
            }
            catch (NotFoundException)
            {
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to validate ip user '{0}:{1}': {2}", tokenData.IdentityProvider, tokenData.UserIdentifier, e);
                throw;
            }

            return user;
        }

        private async Task<DomainUser> GetUserByEmail(TokenData tokenData)
        {
            DomainUser user = null;
            if (string.IsNullOrEmpty(tokenData.Email))
            {
                return null;
            }

            try
            {
                user = await _userService.FindByEmailAsync(tokenData.Email);
            }
            catch (NotFoundException)
            {
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to find user by email '{0}': {1}", tokenData.Email, e);
                throw;
            }

            return user;
        }

        private async Task<DomainUser> GetCurrentUser()
        {
            DomainUser currentUser = null;
            try
            {
                currentUser = await _userService.GetAsync(UserId);
            }
            catch (NotFoundException)
            {
            }
            return currentUser;
        }


        //
        // GET: /account/logoff

        [Route("logoff", Name = RouteNames.Logoff)]
        [AuthorizeMvc]
        public ActionResult Logoff()
        {
            _authenticationService.Clear();
            return RedirectToAction("Index", "Home");
        }


        //
        // GET: /account/authorize

        [Route("authorize")]
        [HttpGet]
        public async Task<ActionResult> Authorize(string data, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home", new { returnUrl });
            }

            try
            {
                await _authenticationService.SetDataAsync(data);
            }
            catch (Exception e)
            {
                Trace.TraceError("Authentication failed for data '{0}': {1}", data, e);
                return RedirectToAction("Index", "Home", new { returnUrl });
            }

            try
            {
                await _userService.GetAsync(UserId);
            }
            catch (NotFoundException)
            {
                return RedirectToAction("Index", "Home", new { returnUrl });
            }
            catch (Exception e)
            {
                Trace.TraceError("Unable to receive user data by id '{0}': {1}", UserId, e);
                return RedirectToAction("Index", "Home", new { returnUrl });
            }

            Uri returnUri;

            if (!Uri.TryCreate(returnUrl, UriKind.Relative, out returnUri))
            {
                return RedirectToAction("Index", "Home");
            }

            return (ActionResult)RedirectToAction("Index", "Home");
        }

        private async Task<DomainUser> CompleteRegistrationAsync(TokenData tokenData)
        {
            DomainUser user = null;

            // Create new profile
            try
            {
                user = await _userService.AddAsync(CreateDomainUser(tokenData));
            }
            catch (Exception e)
            {
                Trace.TraceError("Authentication failed for '{0}:{1}': {2}", tokenData.IdentityProvider, tokenData.UserIdentifier, e);
                throw;
            }

            // Send registration e-mail
            try
            {
                await _emailNotificationService.SendRegistrationEmailAsync(user);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to send registration email to address {0} for user {1}: {2}", user.Email, user.Id, e);
            }

            // Send notification into social network
            ISocialNetworkNotifier notifier = _notificationFactory.GetNotifier(tokenData.IdentityProvider);
            if (notifier != null)
            {
                try
                {
                    await notifier.SendWelcomeMessageAsync(user, tokenData);
                }
                catch (BadGatewayException)
                {
                }
                catch (Exception e)
                {
                    Trace.TraceError("Failed to send welcome message to user '{0}': {1}", user.Id, e);
                }
            }

            //For statistics
            HttpContext.Items.Add("isRegister", true);

            return user;
        }


        // GET: /account/activate-client/?code={code}

        [Route("activate-client")]
        [HttpGet]
        public async Task<ActionResult> ActivateClient(string code)
        {
            DomainPendingClient model;

            try
            {
                model = await _pendingClientService.GetAndDeleteAsync(code);
            }
            catch (NotFoundException)
            {
                return (ActionResult)RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to find client: {0}", e);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            // Add profile
            var user = new DomainUser
            {
                ApplicationName = AppName,
                Name = model.ContactPerson,
                Country = model.Country,
                UserAgent = _productIdExtractor.Get(Request.UserAgent),
                Email = model.Email,
                Roles = new List<string> { DomainRoles.Client }
            };

            try
            {
                user = await _userService.AddAsync(user);
            }
            catch (ConflictException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to create user profile for {0}: {1}", model.Email, e);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            // Set user password
            try
            {
                await _passwordService.SetPasswordAsync(user.Id, model.Password, model.PasswordSalt);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to set user {0} password: {1}", user.Id, e);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            // Add company
            var company = new DomainCompany
            {
                Email = model.Email,
                Name = model.CompanyName,
                Address = model.Address,
                ZipCode = model.ZipCode,
                Phone = model.PhoneNumber,
                Country = model.Country,
                Ein = model.Ein,
                Users = new List<string> { user.Id }
            };

            try
            {
                await _companyService.AddAsync(company);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to create company for user {0}: {1}", user.Id, e);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            // Authenticate
            await _authenticationService.SetUserAsync(user, null, true);

            return RedirectToRoute(RouteNames.ClientSubscriptions);
        }

        private DomainUser CreateDomainUser(TokenData tokenData)
        {
            return new DomainUser
            {
                ApplicationName = AppName,
                Name = tokenData.Name,
                Email = tokenData.Email,
                Memberships = new List<UserMembership>
                {
                    new UserMembership
                    {
                        IdentityProvider = tokenData.IdentityProvider,
                        UserIdentifier = tokenData.UserIdentifier
                    }
                },
                Roles = new List<string> { DomainRoles.User }
            };
        }
    }
}