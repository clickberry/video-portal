// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using Authentication;
using Authentication.IdentityProviders;
using Configuration;
using Portal.Domain.ProfileContext;
using Portal.Web.Infrastructure;

namespace Portal.Web.Controllers
{
    [RoutePrefix("wsfederation")]
    public class WsFederationController : Controller
    {
        private readonly IIdentityFactory _identityFactory;
        private readonly IPortalFrontendSettings _settings;

        public WsFederationController(IIdentityFactory identityFactory, IPortalFrontendSettings settings)
        {
            _identityFactory = identityFactory;
            _settings = settings;
        }


        //
        // GET: /wsfederation/twittermetadata

        [Route("twittermetadata")]
        public ActionResult TwitterMetadata()
        {
            string portalUri = _settings.PortalUri;
            var endpoint = new UriBuilder(portalUri)
            {
                Path = (Url.Action("twitter") ?? string.Empty).ToLowerInvariant(),
                Query = string.Empty
            };

            IMetadataProvider metadataProvider = _identityFactory.CreateMetadataProvider(ProviderType.Twitter);
            string metadata = metadataProvider.GetFederationMetadata(endpoint.Uri);

            return new XmlResult(metadata);
        }


        //
        // GET: /wsfederation/vkmetadata

        [Route("vkmetadata")]
        public ActionResult VkMetadata()
        {
            string portalUri = _settings.PortalUri;
            var endpoint = new UriBuilder(portalUri)
            {
                Path = (Url.Action("vk") ?? string.Empty).ToLowerInvariant(),
                Query = string.Empty
            };

            IMetadataProvider metadataProvider = _identityFactory.CreateMetadataProvider(ProviderType.Vk);
            string metadata = metadataProvider.GetFederationMetadata(endpoint.Uri);

            return new XmlResult(metadata);
        }


        //
        // GET: /wsfederation/okmetadata

        [Route("okmetadata")]
        public ActionResult OkMetadata()
        {
            string portalUri = _settings.PortalUri;
            var endpoint = new UriBuilder(portalUri)
            {
                Path = (Url.Action("ok") ?? string.Empty).ToLowerInvariant(),
                Query = string.Empty
            };

            IMetadataProvider metadataProvider = _identityFactory.CreateMetadataProvider(ProviderType.Odnoklassniki);
            string metadata = metadataProvider.GetFederationMetadata(endpoint.Uri);

            return new XmlResult(metadata);
        }


        //
        // GET: /wsfederation/twitter

        [Route("twitter")]
        [HttpGet]
        public ActionResult Twitter()
        {
            Dictionary<string, string> parameters = Request.Params.ToDictionary();

            string portalUri = _settings.PortalUri;
            var callback = new UriBuilder(portalUri)
            {
                Path = (Url.Action("twittercallback") ?? string.Empty).ToLowerInvariant()
            };

            IIdentityProvider identityProvider = _identityFactory.CreateIdentityProvider(ProviderType.Twitter);
            Uri authenticationUri;

            try
            {
                authenticationUri = identityProvider.GetAutheticationUri(parameters, callback.Uri);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to receive authentication uri: {0}", e);
                return RedirectToAction("Index", "Home");
            }

            return Redirect(authenticationUri.AbsoluteUri);
        }


        //
        // GET: /wsfederation/vk

        [Route("vk")]
        [HttpGet]
        public ActionResult Vk()
        {
            Dictionary<string, string> parameters = Request.Params.ToDictionary();

            string portalUri = _settings.PortalUri;
            var callback = new UriBuilder(portalUri)
            {
                Path = (Url.Action("vkcallback") ?? string.Empty).ToLowerInvariant()
            };

            IIdentityProvider identityProvider = _identityFactory.CreateIdentityProvider(ProviderType.Vk);
            Uri authenticationUri;

            try
            {
                authenticationUri = identityProvider.GetAutheticationUri(parameters, callback.Uri);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to receive authentication uri: {0}", e);
                return RedirectToAction("Index", "Home");
            }

            return Redirect(authenticationUri.AbsoluteUri);
        }


        //
        // GET: /wsfederation/ok

        [Route("ok")]
        [HttpGet]
        public ActionResult Ok()
        {
            Dictionary<string, string> parameters = Request.Params.ToDictionary();

            string portalUri = _settings.PortalUri;
            var callback = new UriBuilder(portalUri)
            {
                Path = (Url.Action("okcallback") ?? string.Empty).ToLowerInvariant()
            };

            IIdentityProvider identityProvider = _identityFactory.CreateIdentityProvider(ProviderType.Odnoklassniki);
            Uri authenticationUri;

            try
            {
                authenticationUri = identityProvider.GetAutheticationUri(parameters, callback.Uri);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to receive authentication uri: {0}", e);
                return RedirectToAction("Index", "Home");
            }

            return Redirect(authenticationUri.AbsoluteUri);
        }


        //
        // GET: /wsfederation/twittercallback

        [Route("twittercallback")]
        [HttpGet]
        public ActionResult TwitterCallback()
        {
            Dictionary<string, string> parameters = Request.Params.ToDictionary();
            if (parameters.ContainsKey("denied"))
            {
                return RedirectToAction("Index", "Home");
            }

            // Create request message
            string portalUri = _settings.PortalUri;
            var signInUri = new UriBuilder(portalUri)
            {
                Path = (Url.Action("twitter") ?? string.Empty).ToLowerInvariant()
            };

            IIdentityProvider identityProvider = _identityFactory.CreateIdentityProvider(ProviderType.Twitter);
            string result;

            try
            {
                result = identityProvider.GetResponseHtml(parameters, signInUri.Uri);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to create html response for twitter: {0}", e);
                return RedirectToAction("Index", "Home");
            }

            return new HtmlResult(result);
        }


        //
        // GET: /wsfederation/vkcallback

        [Route("vkcallback")]
        [HttpGet]
        public ActionResult VkCallback()
        {
            Dictionary<string, string> parameters = Request.Params.ToDictionary();

            if (parameters.ContainsKey("error"))
            {
                return RedirectToAction("Index", "Home");
            }

            // Create request message
            string portalUri = _settings.PortalUri;
            var signInUri = new UriBuilder(portalUri)
            {
                Path = (Url.Action("vk") ?? string.Empty).ToLowerInvariant()
            };

            IIdentityProvider identityProvider = _identityFactory.CreateIdentityProvider(ProviderType.Vk);
            string result;

            try
            {
                result = identityProvider.GetResponseHtml(parameters, signInUri.Uri);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to create html response for vk: {0}", e);
                return RedirectToAction("Index", "Home");
            }

            return new HtmlResult(result);
        }


        //
        // GET: /wsfederation/okcallback

        [Route("okcallback")]
        [HttpGet]
        public ActionResult OkCallback()
        {
            Dictionary<string, string> parameters = Request.Params.ToDictionary();

            if (parameters.ContainsKey("error"))
            {
                return RedirectToAction("Index", "Home");
            }

            // Create request message
            string portalUri = _settings.PortalUri;
            var signInUri = new UriBuilder(portalUri)
            {
                Path = (Url.Action("ok") ?? string.Empty).ToLowerInvariant()
            };

            IIdentityProvider identityProvider = _identityFactory.CreateIdentityProvider(ProviderType.Odnoklassniki);
            string result;

            try
            {
                result = identityProvider.GetResponseHtml(parameters, signInUri.Uri);
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to create html response for ok: {0}", e);
                return RedirectToAction("Index", "Home");
            }

            return new HtmlResult(result);
        }
    }
}