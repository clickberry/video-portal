// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Portal.Web.Infrastructure
{
    public static class HtmlExtensions
    {
        /// <summary>
        ///     Create action link with a html content.
        /// </summary>
        /// <param name="helper">UrlHelper.</param>
        /// <param name="content">Content string.</param>
        /// <param name="actionName">Action name.</param>
        /// <param name="controllerName">Controller name.</param>
        /// <param name="routeValues">Route values.</param>
        /// <param name="hash">Url hash code.</param>
        /// <param name="htmlAttributes">Html attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString ContentLink(this UrlHelper helper, string content, string actionName, string controllerName,
            object routeValues = null, string hash = null,
            IDictionary<string, object> htmlAttributes = null, bool highlight = false)
        {
            StringBuilder attributesString;

            if (htmlAttributes != null)
            {
                attributesString = new StringBuilder(" ");

                foreach (var attribute in htmlAttributes)
                {
                    attributesString.AppendFormat(@"{0}=""{1}""", attribute.Key, attribute.Value);
                }
            }
            else
            {
                attributesString = new StringBuilder();
            }

            string uri = helper.Action(actionName, controllerName, routeValues);
            if (!string.IsNullOrEmpty(uri))
            {
                uri = uri.TrimEnd(new[] { '/' });
            }

            hash = string.IsNullOrEmpty(hash) ? string.Empty : "/#" + hash;

            string @class = highlight &&
                            string.Compare(helper.RequestContext.RouteData.Values["controller"].ToString(), controllerName, StringComparison.OrdinalIgnoreCase) == 0 &&
                            string.Compare(helper.RequestContext.RouteData.Values["action"].ToString(), actionName, StringComparison.OrdinalIgnoreCase) == 0
                ? @" class=""active"""
                : string.Empty;

            return MvcHtmlString.Create(String.Format(@"<a href=""{0}{3}""{1}{4}>{2}</a>", uri, attributesString, content, hash, @class));
        }

        /// <summary>
        ///     Gets a value indicating whether current page is authentication page.
        /// </summary>
        /// <returns>Selected video.</returns>
        public static bool IsAuthenticationPage(this ViewContext viewContext)
        {
            string controller = viewContext.RouteData.Values["controller"].ToString();
            string action = viewContext.RouteData.Values["action"].ToString();

            return string.Compare(controller, "home", StringComparison.OrdinalIgnoreCase) == 0 &&
                   string.Compare(action, "index", StringComparison.OrdinalIgnoreCase) == 0 &&
                   !viewContext.HttpContext.Request.IsAuthenticated;
        }

        public static MvcHtmlString HashTags(this HtmlHelper html, string str, string linkPrefix)
        {
            if (string.IsNullOrEmpty(str))
            {
                return MvcHtmlString.Empty;
            }

            if (string.IsNullOrEmpty(linkPrefix))
            {
                linkPrefix = "#";
            }
            else if (!linkPrefix.EndsWith("/"))
            {
                linkPrefix += "/";
            }

            var result = Regex.Replace(str, "#([^#\\s]+)", "<a href=\"" + linkPrefix + "$1\" target=\"_self\">#$1</a>");
            return new MvcHtmlString(result);
        }
    }
}