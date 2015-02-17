// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Authentication.IdentityProviders
{
    public interface IIdentityProvider
    {
        Uri GetAutheticationUri(IDictionary<string, string> parameters, Uri callback);

        string GetResponseHtml(IDictionary<string, string> parameters, Uri signinUri);
    }
}