// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.RegularExpressions;
using Portal.BLL.Infrastructure;

namespace Portal.BLL.Concrete.Infrastructure
{
    public sealed class UserAgentVerifier : IUserAgentVerifier
    {
        private static readonly Regex MobileDevice =
            new Regex("Mobile|iP(hone|od|ad)|Android|BlackBerry|IEMobile|Kindle|NetFront|Silk-Accelerated|(hpw|web)OS|Fennec|Minimo|Opera M(obi|ini)|Blazer|Dolfin|Dolphin|Skyfire|Zune",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private static readonly Regex Bot = new Regex("(.*(https?://|google|bot|yahoo|bing|yandex|vk.com/dev/Share|facebookexternalhit))",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        public bool IsMobileDevice(string userAgent)
        {
            return MobileDevice.IsMatch(userAgent);
        }

        public bool IsSocialBot(string userAgent)
        {
            return userAgent.Contains("vk.com/dev/Share") ||
                   userAgent.Contains("facebookexternalhit");
        }

        public bool IsBot(string userAgent)
        {
            return Bot.IsMatch(userAgent);
        }

        public Device GetDevice(string userAgent)
        {
            var device = new Device
            {
                IsMobile = MobileDevice.IsMatch(userAgent),
                Browser = GetBrowser(userAgent)
            };

            device.Platform = GetPlatform(userAgent, device.IsMobile);

            return device;
        }

        private PlatformType GetPlatform(string userAgent, bool isMobile)
        {
            if (isMobile)
            {
                if (userAgent.IndexOf("windows phone", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return PlatformType.WindowsPhone;
                }
                if (userAgent.IndexOf("android", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return PlatformType.Android;
                }
                if (userAgent.IndexOf("iphone", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return PlatformType.Iphone;
                }
                if (userAgent.IndexOf("ipad", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return PlatformType.Ipad;
                }
                if (userAgent.IndexOf("baclberry", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return PlatformType.Blackberry;
                }
            }
            else
            {
                if (userAgent.IndexOf("windows", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return PlatformType.Windows;
                }
                if (userAgent.IndexOf("macintosh", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return PlatformType.Macintosh;
                }
                if (userAgent.IndexOf("linux", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return PlatformType.Linux;
                }
            }

            return PlatformType.Unrecognized;
        }

        private BrowserType GetBrowser(string userAgent)
        {
            if (userAgent.IndexOf("firefox", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return BrowserType.Firefox;
            }
            if (userAgent.IndexOf("safari", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (userAgent.IndexOf("opr/", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return BrowserType.OperaNew;
                }
                if (userAgent.IndexOf("chrome", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return BrowserType.Chrome;
                }
                return BrowserType.Safari;
            }
            if (userAgent.IndexOf("msie", StringComparison.OrdinalIgnoreCase) >= 0 ||
                userAgent.IndexOf("trident", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return BrowserType.InternetExplorer;
            }
            if (userAgent.IndexOf("opera", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return BrowserType.Opera;
            }

            return BrowserType.Unrecognized;
        }
    }
}