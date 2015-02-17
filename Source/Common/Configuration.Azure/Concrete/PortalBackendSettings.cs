// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Configuration.Azure.Concrete
{
    public sealed class PortalBackendSettings : PortalSettings, IPortalBackendSettings
    {
        private readonly IConfigurationProvider _provider;

        public PortalBackendSettings(IConfigurationProvider provider) : base(provider)
        {
            _provider = provider;
        }

        public string BaseUrl
        {
            get { return _provider.Get("BaseUrl"); }
        }

        public string BearerToken
        {
            get { return _provider.Get("BearerToken"); }
        }

        public int Period
        {
            get { return _provider.Get<int>("Period"); }
        }

        public string FfmpegVersion
        {
            get { return _provider.Get("FfmpegVersion"); }
        }

        public string TaskTypes
        {
            get { return _provider.Get("TaskTypes"); }
        }

        public string BackendId
        {
            get { return _provider.Get("BackendId"); }
        }
    }
}