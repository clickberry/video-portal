// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asp.Infrastructure.Modules.UriModification
{
    /// <summary>
    ///     Forces https, removes www.
    /// </summary>
    public class UriModificationModule : IHttpModule
    {
        private List<IUriModifier> _modifiers;

        public String ModuleName
        {
            get { return "UriModificationModule"; }
        }

        public void Init(HttpApplication context)
        {
            _modifiers = new List<IUriModifier>
            {
                new PrefixUriModifier()
            };

            context.BeginRequest += ContextBeginRequest;
        }

        public void Dispose()
        {
        }

        private void ContextBeginRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            HttpContext context = application.Context;
            Uri url = context.Request.Url;

            // Modifies request URL if required
            Uri uri = _modifiers.Aggregate(url, (current, modifier) => modifier.Process(current));

            if (context.Request.Url != uri)
            {
                context.Response.Redirect(uri.AbsoluteUri, true);
            }
        }
    }
}