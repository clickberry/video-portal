// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Configuration.Azure.Concrete
{
    public class PortalSettings : IPortalSettings
    {
        private readonly IConfigurationProvider _provider;

        public PortalSettings(IConfigurationProvider provider)
        {
            _provider = provider;
        }

        public string DiagnosticsConnectionString
        {
            get { return _provider.Get("DiagnosticsConnectionString"); }
        }

        public string MongoConnectionString
        {
            get { return _provider.Get("MongoConnectionString"); }
        }

        public string PortalUri
        {
            get { return _provider.Get("PortalUri"); }
        }

        public string JiraIssueCollectorUri
        {
            get { return _provider.Get("JiraIssueCollectorUri"); }
        }

        public string DataConnectionString
        {
            get { return _provider.Get("DataConnectionString"); }
        }

        public string EmailAddressAlerts
        {
            get { return _provider.Get("EmailAddressAlerts"); }
        }

        public string EmailAddressErrors
        {
            get { return _provider.Get("EmailAddressErrors"); }
        }

        public string EmailSubjectErrors
        {
            get { return _provider.Get("EmailSubjectErrors"); }
        }

        public MailSettings MailSettings
        {
            get
            {
                var result = new MailSettings();

                string setting = _provider.Get("MailSettings");
                if (!string.IsNullOrEmpty(setting))
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<MailSettings>(setting);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Failed to parse MailSettings setting: {0}", e);
                    }
                }

                return result;
            }
        }
    }
}