// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.RegularExpressions;

namespace Asp.Infrastructure.Modules.UriModification
{
    public sealed class HttpsUriModifier : IUriModifier
    {
        // We should redirect user while navigating to the private data
        private readonly Regex _httpsRequired = new Regex("^/(profile)|(account)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Uri Process(Uri uri)
        {
            if (_httpsRequired.IsMatch(uri.AbsolutePath) && !string.Equals(uri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
            {
                int port = uri.Port == 82 ? 444 : 443;
                return new Uri(string.Format("https://{0}:{1}{2}", uri.Host, port, uri.PathAndQuery));
            }

            return uri;
        }
    }
}