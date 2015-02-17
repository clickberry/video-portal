// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using Configuration.Azure.Concrete;

namespace Asp.Infrastructure.Modules.HeadersModification
{
    /// <summary>
    ///     Modifies response headers.
    /// </summary>
    public sealed class HeadersModificationModule : IHttpModule
    {
        private const int MaxAgeDays = 365;
        private const string VersionHeader = "x-cb-version";
        private const string RoleIdHeader = "x-cb-instance";
        private readonly Regex _instanceIdFormat = new Regex(@".+_IN_(\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex _versionFormat = new Regex(@"v(\d+\.\d+\.\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        internal string Version;
        private string _roleId;

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += PreSendRequestHeaders;

            var configurationProvider = new ConfigurationProvider();

            // Get role id
            string roleId = configurationProvider.GetRoleId();
            Match match = _instanceIdFormat.Match(roleId);
            _roleId = match.Success ? match.Groups[1].Value : null;

            // Get assembly version
            Assembly assembly = Assembly.GetExecutingAssembly();
            string version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            // Select version number
            match = _versionFormat.Match(version);
            Version = match.Success ? match.Groups[1].Value : version;
        }

        public void Dispose()
        {
        }

        /// <summary>
        ///     Occurs just before ASP.NET sends HTTP headers to the client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreSendRequestHeaders(object sender, EventArgs e)
        {
            var applicaton = (HttpApplication)sender;

            HttpResponse response = applicaton.Response;

            // Append headers
            response.Headers.Add(VersionHeader, Version);
            if (!string.IsNullOrEmpty(_roleId))
            {
                response.Headers.Add(RoleIdHeader, _roleId);
            }

            // Remove headers
            response.Headers.Remove("Etag");
            response.Headers.Remove("Server");

            // Remove FedAuth cookies
            response.Cookies.Remove("FedAuth");
            response.Cookies.Remove("FedAuth1");

            // CDN special headers
            if (applicaton.Request.Url.AbsolutePath.StartsWith("/cdn", StringComparison.OrdinalIgnoreCase))
            {
                // caching
                response.Cache.SetCacheability(HttpCacheability.Public);
                response.Cache.SetMaxAge(TimeSpan.FromDays(MaxAgeDays));
                response.ExpiresAbsolute = DateTime.UtcNow.AddDays(MaxAgeDays);

                // CORS for web fonts http://blog.feedbackhound.com/hosting-web-fonts-in-azure-blob-storage-using-the-new-cors-support
                response.Headers.Add("Access-Control-Allow-Origin", "*");
            }

            // Favicon cache headers
            if (applicaton.Request.Url.AbsolutePath.StartsWith("/favicon", StringComparison.OrdinalIgnoreCase))
            {
                response.Cache.SetCacheability(HttpCacheability.Public);
                response.Cache.SetMaxAge(TimeSpan.FromDays(MaxAgeDays));
                response.ExpiresAbsolute = DateTime.UtcNow.AddDays(MaxAgeDays);
            }
        }
    }
}