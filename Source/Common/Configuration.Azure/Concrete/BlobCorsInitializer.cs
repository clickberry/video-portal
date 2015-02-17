// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Configuration.Azure.Concrete
{
    /// <summary>
    ///     Enables CORS on blob storage for portal.
    /// </summary>
    public class BlobCorsInitializer : IInitializable
    {
        private readonly CloudBlobClient _blobClient;
        private readonly IPortalSettings _settings;

        public BlobCorsInitializer(IPortalSettings settings, CloudBlobClient blobClient)
        {
            _settings = settings;
            _blobClient = blobClient;
        }

        public void Initialize()
        {
            // Given a BlobClient, download the current Service Properties 
            ServiceProperties blobServiceProperties = _blobClient.GetServiceProperties();

            // Enable and Configure CORS
            ConfigureCors(blobServiceProperties);

            // Commit the CORS changes into the Service Properties
            _blobClient.SetServiceProperties(blobServiceProperties);
        }

        private void ConfigureCors(ServiceProperties serviceProperties)
        {
            serviceProperties.Cors = new CorsProperties();
            serviceProperties.Cors.CorsRules.Add(new CorsRule
            {
                AllowedHeaders = new[] { "*" },
                AllowedMethods = CorsHttpMethods.Get | CorsHttpMethods.Head,
                AllowedOrigins = new[] { "*" },
                ExposedHeaders = new[] { "Cache-Control", "Content-Type", "Last-Modified", "ETag", "Accept-Ranges" },
                MaxAgeInSeconds = 31536000
            });
        }
    }
}