// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IdentityModel.Metadata;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Web.Mvc;
using System.Xml;
using Configuration;

namespace Portal.Web.App_Start
{
    public class FederationMetadataConfig
    {
        private const string WsFederationIssuer = "https://{0}.accesscontrol.windows.net/v2/wsfederation";
        private const string WsFederationMetadata = "https://{0}.accesscontrol.windows.net/FederationMetadata/2007-06/FederationMetadata.xml";

        /// <summary>
        ///     Configure ACS endpoint address in Web.config.
        /// </summary>
        public static void Intialize()
        {
            // Compute federation metadata address based on web config's 
            // <system.identityModel.services>\<federationConfiguration>\<wsFederation issuer=""> attribute value
            var settings = DependencyResolver.Current.GetService<IPortalFrontendSettings>();
            string acsNamespace = settings.AcsNamespace;

            // Setup correct issuer name
            // Compute federation metadata address based on web config's 
            // <system.identityModel.services>\<federationConfiguration>\<wsFederation issuer=""> attribute value
            string stsMetadataAddress = string.Format(WsFederationMetadata, acsNamespace);

            // Update the web config with latest local STS federation meta data each
            // time when instance of the application is created
            try
            {
                using (XmlReader metadataReader = XmlReader.Create(stsMetadataAddress))
                {
                    // Creates the xml reader pointing to the updated web.config contents
                    // Don't validate the cert signing the federation metadata
                    var serializer = new MetadataSerializer
                    {
                        CertificateValidationMode = X509CertificateValidationMode.None,
                    };

                    var metadata = (EntityDescriptor)serializer.ReadMetadata(metadataReader);

                    // Select security token descriptors
                    SecurityTokenServiceDescriptor descriptor = metadata.RoleDescriptors.OfType<SecurityTokenServiceDescriptor>().FirstOrDefault();
                    if (descriptor != null)
                    {
                        var issuerNameRegistry = new ConfigurationBasedIssuerNameRegistry();

                        // Try to find and add certificates for trusted issuers
                        foreach (KeyDescriptor keyDescriptor in descriptor.Keys)
                        {
                            if (keyDescriptor.KeyInfo != null && (keyDescriptor.Use == KeyType.Signing || keyDescriptor.Use == KeyType.Unspecified))
                            {
                                SecurityKeyIdentifier keyInfo = keyDescriptor.KeyInfo;
                                X509RawDataKeyIdentifierClause clause;
                                keyInfo.TryFind(out clause);

                                if (clause != null)
                                {
                                    var x509Certificate2 = new X509Certificate2(clause.GetX509RawData());
                                    if (x509Certificate2.Thumbprint != null)
                                    {
                                        issuerNameRegistry.AddTrustedIssuer(x509Certificate2.Thumbprint, x509Certificate2.SubjectName.Name);
                                    }
                                }
                            }
                        }

                        // Set retrieved issuers
                        FederatedAuthentication.FederationConfiguration.IdentityConfiguration.IssuerNameRegistry = issuerNameRegistry;
                    }
                    metadataReader.Dispose();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Error occured when update web config with latest local STS federation meta data. {0}", e);
                return;
            }

            // Add audience URIs
            string portalUri = settings.PortalUri;

            // Front-end
            var uriBuilder = new UriBuilder(portalUri);
            FederatedAuthentication.FederationConfiguration.IdentityConfiguration.AudienceRestriction.AllowedAudienceUris.Add(uriBuilder.Uri);

            // Extension
            uriBuilder.Path = "/extension/login";
            FederatedAuthentication.FederationConfiguration.IdentityConfiguration.AudienceRestriction.AllowedAudienceUris.Add(uriBuilder.Uri);

            // Update WS-Federation configuration
            FederatedAuthentication.WSFederationAuthenticationModule.Issuer = string.Format(WsFederationIssuer, acsNamespace);
            FederatedAuthentication.WSFederationAuthenticationModule.Realm = portalUri;
        }
    }
}