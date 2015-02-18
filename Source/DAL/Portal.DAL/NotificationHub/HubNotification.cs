// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.DAL.NotificationHub
{
    public sealed class HubNotification
    {
        public HubNotification(string header)
        {
            if (string.IsNullOrEmpty(header))
            {
                throw new ArgumentNullException("header");
            }

            Header = header;
        }

        public string Header { get; private set; }
    }
}