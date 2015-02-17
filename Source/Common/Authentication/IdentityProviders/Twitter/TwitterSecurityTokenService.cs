// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Services;
using System.Security.Claims;
using Configuration;
using TweetSharp;

namespace Authentication.IdentityProviders.Twitter
{
    public sealed class TwitterSecurityTokenService : SecurityTokenService, IIdentityProvider
    {
        private const string WtRealm = "https://{0}.accesscontrol.windows.net/";
        private const string WReply = "https://{0}.accesscontrol.windows.net/v2/wsfederation";
        private const string TwitterAccountPage = "https://twitter.com/account/redirect_by_id?id={0}";
        private readonly IPortalFrontendSettings _settings;

        public TwitterSecurityTokenService(SecurityTokenServiceConfiguration configuration, IPortalFrontendSettings settings)
            : base(configuration)
        {
            _settings = settings;
        }

        public Uri GetAutheticationUri(IDictionary<string, string> parameters, Uri callback)
        {
            var callbackUri = new UriBuilder(callback)
            {
                Query = string.Format("context={0}", parameters["wctx"])
            };

            // Pass your credentials to the service
            string consumerKey = _settings.TwitterConsumerKey;
            string consumerSecret = _settings.TwitterConsumerSecret;

            var service = new TwitterService(consumerKey, consumerSecret);

            // Retrieve an OAuth Request Token
            string returnUri = callbackUri.ToString();
            OAuthRequestToken requestToken;

            try
            {
                requestToken = service.GetRequestToken(returnUri);
            }
            catch (ArgumentException e)
            {
                string message = string.Format("Failed to request twitter auth token, code: {0}, exception: {1}", service.Response.StatusCode, service.Response.InnerException);
                throw new ArgumentException(message, e);
            }

            // Redirect to the OAuth Authorization URL
            return service.GetAuthorizationUri(requestToken);
        }

        public string GetResponseHtml(IDictionary<string, string> parameters, Uri signinUri)
        {
            var requestToken = new OAuthRequestToken { Token = parameters["oauth_token"] };

            // Exchange the Request Token for an Access Token
            string consumerKey = _settings.TwitterConsumerKey;
            string consumerSecret = _settings.TwitterConsumerSecret;

            var service = new TwitterService(consumerKey, consumerSecret);

            OAuthAccessToken accessToken = service.GetAccessToken(requestToken, parameters["oauth_verifier"]);
            service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);

            TwitterUser user = service.GetUserProfile(new GetUserProfileOptions());

            // Claims
            string name = user != null ? user.Name : accessToken.ScreenName;
            string nameIdentifier = string.Format(TwitterAccountPage, accessToken.UserId);
            string token = accessToken.Token;
            string tokenSecret = accessToken.TokenSecret;

            string acsNamespace = _settings.AcsNamespace;

            string wtRealm = string.Format(WtRealm, acsNamespace);
            string wReply = string.Format(WReply, acsNamespace);

            var requestMessage = new SignInRequestMessage(signinUri, wtRealm, wReply);

            // Add extracted claims
            var identity = new ClaimsIdentity(AuthenticationTypes.Federation);
            identity.AddClaim(new Claim(ClaimTypes.Name, name));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
            identity.AddClaim(new Claim(TwitterClaims.TwitterToken, token));
            identity.AddClaim(new Claim(TwitterClaims.TwitterTokenSecret, tokenSecret));

            var principal = new ClaimsPrincipal(identity);

            SignInResponseMessage responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, principal, this);
            responseMessage.Context = parameters["context"];

            return responseMessage.WriteFormPost();
        }

        protected override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            if (request.AppliesTo == null)
            {
                throw new InvalidRequestException("The AppliesTo is null.");
            }

            var scope = new Scope(request.AppliesTo.Uri.OriginalString, SecurityTokenServiceConfiguration.SigningCredentials)
            {
                TokenEncryptionRequired = false,
                SymmetricKeyEncryptionRequired = false
            };

            scope.ReplyToAddress = string.IsNullOrEmpty(request.ReplyTo) ? scope.AppliesToAddress : request.ReplyTo;

            return scope;
        }

        protected override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            if (principal == null)
            {
                throw new InvalidRequestException("The caller's principal is null.");
            }

            var claimsIdentity = principal.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                throw new InvalidRequestException("The caller's identity is invalid.");
            }

            return claimsIdentity;
        }
    }
}