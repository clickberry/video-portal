// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Configuration.Azure.Concrete
{
    public sealed class PortalMiddleendSettings : PortalSettings, IPortalMiddleendSettings
    {
        private readonly IConfigurationProvider _provider;

        public PortalMiddleendSettings(IConfigurationProvider provider) : base(provider)
        {
            _provider = provider;
        }

        public string BearerToken
        {
            get { return _provider.Get("BearerToken"); }
        }

        public bool MongoAutomigrationEnabled
        {
            get { return _provider.Get<bool>("MongoAutomigrationEnabled"); }
        }

        public string DefaultAdministrator
        {
            get { return _provider.Get("DefaultAdministrator"); }
        }
    }
}