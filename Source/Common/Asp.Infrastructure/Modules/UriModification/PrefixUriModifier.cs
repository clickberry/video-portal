// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Asp.Infrastructure.Modules.UriModification
{
    public sealed class PrefixUriModifier : IUriModifier
    {
        public Uri Process(Uri uri)
        {
            if (uri.Authority.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
            {
                return new Uri(string.Format("{0}://{1}{2}", uri.Scheme, uri.Authority.Substring(4), uri.PathAndQuery));
            }

            return uri;
        }
    }
}