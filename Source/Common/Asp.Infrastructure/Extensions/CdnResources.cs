// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;
using Configuration;

namespace Asp.Infrastructure.Extensions
{
    /// <summary>
    ///     Converts a relative urls into cdn.
    /// </summary>
    public static class CdnResources
    {
        private static readonly Regex CdnTextFormat = new Regex("(\"(/cdn/)[^\"]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex CdnUrlFormat = new Regex("^((/cdn/).+)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex VersionFormat = new Regex(@"v(\d+\.\d+\.\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static IPortalFrontendSettings _settings;
        internal static string Version;

        static CdnResources()
        {
            // Get assembly version
            Assembly assembly = Assembly.GetExecutingAssembly();
            string version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            // Select version number
            Match match = VersionFormat.Match(version);
            Version = match.Success ? match.Groups[1].Value : Guid.NewGuid().ToString("N");
        }

        /// <summary>
        ///     Set configuration provider.
        /// </summary>
        /// <param name="settings"></param>
        public static void SetConfigurationProvider(IPortalFrontendSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        ///     Renders scripts.
        /// </summary>
        /// <param name="paths">Paths.</param>
        /// <returns>Result.</returns>
        public static IHtmlString RenderScripts(params string[] paths)
        {
            string result = ProcessUrls(Scripts.Render(paths).ToString(), false);
            return new HtmlString(result);
        }

        /// <summary>
        ///     Renders styles.
        /// </summary>
        /// <param name="paths">Paths.</param>
        /// <returns>Result.</returns>
        public static IHtmlString RenderStyles(params string[] paths)
        {
            string result = ProcessUrls(Styles.Render(paths).ToString(), false);
            return new HtmlString(result);
        }

        /// <summary>
        ///     Replaces resource path:
        ///     ~/cdn/script.js -> //cdn.microsoft.com/script.js
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string ProcessUrls(string data, bool url)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            if (data.StartsWith("~"))
            {
                data = data.Substring(1);
            }

            if (_settings == null)
            {
                return data;
            }

            string cdnUri = _settings.CdnUri;
            if (string.IsNullOrEmpty(cdnUri))
            {
                return data;
            }

            var regex = url ? CdnUrlFormat : CdnTextFormat;
            string replacement = string.Format("//{0}/", cdnUri);
            return regex.Replace(data, p =>
            {
                string uri = p.Groups[1].Value.Replace(p.Groups[2].Value, replacement);
                char joinChar = '?';

                int queryStart = uri.IndexOf("?", StringComparison.Ordinal);
                if (queryStart > 0)
                {
                    if (uri.IndexOf("v=", StringComparison.Ordinal) > 0)
                    {
                        return uri;
                    }

                    joinChar = '&';
                }

                return string.Format("{0}{1}v={2}", uri, joinChar, Version);
            });
        }

        /// <summary>
        ///     Renders an url.
        /// </summary>
        /// <param name="resourceUrl"></param>
        /// <returns></returns>
        public static string RenderUrl(string resourceUrl)
        {
            return ProcessUrls(resourceUrl, true);
        }
    }
}