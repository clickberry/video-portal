﻿// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Text;
using System.Web.Mvc;

namespace Portal.Web.Infrastructure
{
    public class HtmlResult : ContentResult
    {
        public HtmlResult(string html)
        {
            Content = html;
            ContentEncoding = Encoding.UTF8;
            ContentType = "text/html";
        }
    }
}