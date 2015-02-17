// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Portal.BLL.Infrastructure;

namespace Portal.BLL.Concrete.Infrastructure
{
    /// <summary>
    ///     Handles project uri generation.
    /// </summary>
    public sealed class ProjectUriProvider : IProjectUriProvider
    {
        private readonly int[] _emulatorPorts = { 81, 444 };

        /// <summary>
        /// </summary>
        /// <param name="portalUri">
        ///     Use string for config-agnostic parametrization. This provider also used in LinkTracker
        ///     solution.
        /// </param>
        public ProjectUriProvider(string portalUri)
        {
            BaseUri = new Uri(portalUri);
        }

        public Uri BaseUri { get; set; }

        public string GetUri(string projectId)
        {
            var uri = new UriBuilder(Uri.UriSchemeHttp, BaseUri.Host, BaseUri.Port, string.Format("/video/{0}", projectId));

            // Http for emulator
            uri.Port = _emulatorPorts.Contains(uri.Port) ? 81 : 80;

            return uri.Uri.ToString();
        }
    }
}