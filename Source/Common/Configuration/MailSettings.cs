// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Configuration
{
    public sealed class MailSettings
    {
        public bool EnableSsl { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public int Timeout { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}