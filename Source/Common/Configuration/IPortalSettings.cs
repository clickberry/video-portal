// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Configuration
{
    public interface IPortalSettings
    {
        /// <summary>
        ///     Diagnostics connection string.
        /// </summary>
        string DiagnosticsConnectionString { get; }

        string MongoConnectionString { get; }

        /// <summary>
        ///     URI of the current portal.
        /// </summary>
        string PortalUri { get; }

        string JiraIssueCollectorUri { get; }

        /// <summary>
        ///     Azure storage connection string.
        /// </summary>
        string DataConnectionString { get; }

        /// <summary>
        ///     Defines email to send alerts from.
        /// </summary>
        string EmailAddressAlerts { get; }

        /// <summary>
        ///     Email address for error collection.
        /// </summary>
        string EmailAddressErrors { get; }

        /// <summary>
        ///     Subject for error emails.
        /// </summary>
        string EmailSubjectErrors { get; }

        /// <summary>
        ///     SMTP server settings.
        /// </summary>
        MailSettings MailSettings { get; }
    }
}