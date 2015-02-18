// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Reflection;
using Configuration.Azure.TraceListeners;
using Portal.BLL.Services;

namespace Configuration.Azure.Concrete
{
    public sealed class TraceListenersInitializer : IInitializable
    {
        private readonly IEmailSenderService _emailService;
        private readonly IPortalSettings _portalSettings;

        public TraceListenersInitializer(IPortalSettings portalSettings, IEmailSenderService emailService)
        {
            _portalSettings = portalSettings;
            _emailService = emailService;
        }

        public void Initialize()
        {
            // Portal URL
            string portalUri = _portalSettings.PortalUri;
            if (string.IsNullOrEmpty(portalUri))
            {
                throw new ArgumentNullException("PortalUri");
            }

            if (!Uri.IsWellFormedUriString(portalUri, UriKind.Absolute))
            {
                throw new UriFormatException("PortalUri");
            }

            string environment = new Uri(portalUri).Host;

            // Portal version
            Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
            string portalVersion = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            // Add e-mail trace listener
            Trace.Listeners.Add(new EmailTraceListener(environment, portalVersion, _emailService, _portalSettings));

            // Issue collector URL
            string jiraIssueCollectorUri = _portalSettings.JiraIssueCollectorUri;
            if (string.IsNullOrEmpty(jiraIssueCollectorUri) || !Uri.IsWellFormedUriString(jiraIssueCollectorUri, UriKind.Absolute))
            {
                // If it's null skip initialization
                return;
            }

            // Add jira trace listener
            Trace.Listeners.Add(new JiraTraceListener(jiraIssueCollectorUri, environment, portalVersion));
        }
    }
}