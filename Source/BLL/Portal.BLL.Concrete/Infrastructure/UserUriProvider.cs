// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Linq;
using Portal.BLL.Infrastructure;

namespace Portal.BLL.Concrete.Infrastructure
{
    /// <summary>
    ///     Handles user uri generation.
    /// </summary>
    public sealed class UserUriProvider : IUserUriProvider
    {
        private readonly int[] _emulatorPorts = { 81, 444 };

        public UserUriProvider(string portalUri)
        {
            BaseUri = new Uri(portalUri);
        }

        public Uri BaseUri { get; set; }

        public string GetUri(string projectId)
        {
            var uri = new UriBuilder(Uri.UriSchemeHttp, BaseUri.Host, BaseUri.Port, string.Format("/user/{0}/videos", projectId));

            // Http for emulator
            uri.Port = _emulatorPorts.Contains(uri.Port) ? 81 : 80;

            return uri.Uri.ToString();
        }
    }
}