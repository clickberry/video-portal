// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IdentityModel.Configuration;
using System.IdentityModel.Metadata;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Authentication.IdentityProviders.Ok
{
    public class OkSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration, IMetadataProvider
    {
        public OkSecurityTokenServiceConfiguration()
        {
            TokenIssuerName = OkClaims.IssuerName;
            string signingCertificatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.SigningCertificate);
            var signignCert = new X509Certificate2(signingCertificatePath, Constants.SigningCertificatePassword, X509KeyStorageFlags.PersistKeySet);
            SigningCredentials = new X509SigningCredentials(signignCert);
            ServiceCertificate = signignCert;

            SecurityTokenService = typeof (OkSecurityTokenService);
        }

        public string GetFederationMetadata(Uri endpoint)
        {
            // hostname
            var passiveEndpoint = new EndpointReference(endpoint.AbsoluteUri);
            var activeEndpoint = new EndpointReference(endpoint.AbsoluteUri);

            // metadata document 
            var entity = new EntityDescriptor(new EntityId(OkClaims.IssuerName));
            var sts = new SecurityTokenServiceDescriptor();
            entity.RoleDescriptors.Add(sts);

            // signing key
            var signingKey = new KeyDescriptor(SigningCredentials.SigningKeyIdentifier) { Use = KeyType.Signing };
            sts.Keys.Add(signingKey);

            // claim types
            sts.ClaimTypesOffered.Add(new DisplayClaim(ClaimTypes.Name, "Name", "User name"));
            sts.ClaimTypesOffered.Add(new DisplayClaim(ClaimTypes.NameIdentifier, "Name Identifier", "User name identifier"));
            sts.ClaimTypesOffered.Add(new DisplayClaim(OkClaims.OkToken, "Token", "Service token"));

            // passive federation endpoint
            sts.PassiveRequestorEndpoints.Add(passiveEndpoint);

            // supported protocols

            //Inaccessable due to protection level
            //sts.ProtocolsSupported.Add(new Uri(WSFederationConstants.Namespace));
            sts.ProtocolsSupported.Add(new Uri("http://docs.oasis-open.org/wsfed/federation/200706"));

            // add passive STS endpoint
            sts.SecurityTokenServiceEndpoints.Add(activeEndpoint);

            // metadata signing
            entity.SigningCredentials = SigningCredentials;

            // serialize 
            var serializer = new MetadataSerializer();

            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteMetadata(memoryStream, entity);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}