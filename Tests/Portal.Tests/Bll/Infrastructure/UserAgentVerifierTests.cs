// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Portal.BLL.Concrete.Infrastructure;
using Xunit;

namespace Portal.Tests.Bll.Infrastructure
{
    public sealed class UserAgentVerifierTests
    {
        [Fact]
        public void TestIsAHuman()
        {
            // Arrange
            var verifier = new UserAgentVerifier();
            var userAgents = new[]
            {
                // IE
                "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko",

                // Chrome
                "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.120 Safari/537.36",

                // Firefox
                "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0",

                // Safari
                "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/534.57.2 (KHTML, like Gecko) Version/5.1.7 Safari/534.57.2",

                // Opera
                "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.132 Safari/537.36 OPR/21.0.1432.57 (Edition Campaign 51)", // opera

                // Yandex.Browser
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_2) AppleWebKit/536.5 (KHTML, like Gecko) YaBrowser/1.0.1084.5402 Chrome/19.0.1084.5402 Safari/536.5",
                "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/536.5 (KHTML, like Gecko) YaBrowser/1.0.1084.5402 Chrome/19.0.1084.5402 Safari/536.5",
                "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/536.5 (KHTML, like Gecko) YaBrowser/1.0.1084.5402 Chrome/19.0.1084.5402 Safari/536.5",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.12785 YaBrowser/13.12.1599.12785 Safari/537.36"
            };

            // Act
            IEnumerable<bool> results = userAgents.Select(verifier.IsBot);

            // Assert
            Assert.True(results.All(r => !r));
        }

        [Fact]
        public void TestIsNotAHuman()
        {
            // Arrange
            var verifier = new UserAgentVerifier();

            // list from http://user-agent-string.info/list-of-ua/bots
            var userAgents = new[]
            {
                // Google
                "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)", // Googlebot/2.1
                "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5376e Safari/8536.25 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)",
                // Googlebot-Mobile
                "Googlebot/2.1 (+http://www.google.com/bot.html)", // Googlebot/2.1
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Web Preview) Chrome/27.0.1453 Safari/537.36", // Google Web Preview
                "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5376e Safari/8536.25 (compatible; Googlebot-Mobile/2.1; +http://www.google.com/bot.html)",
                // Googlebot-Mobile
                "Mediapartners-Google", // Mediapartners-Google
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.4 (KHTML, like Gecko; Google Web Preview) Chrome/22.0.1229 Safari/537.4", // Google Web Preview
                "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_1 like Mac OS X; en-us) AppleWebKit/532.9 (KHTML, like Gecko) Version/4.0.5 Mobile/8B117 Safari/6531.22.7 (compatible; Googlebot-Mobile/2.1; +http://www.google.com/bot.html)",
                // Googlebot-Mobile/2.1
                "SAMSUNG-SGH-E250/1.0 Profile/MIDP-2.0 Configuration/CLDC-1.1 UP.Browser/6.2.3.3.c.1.101 (GUI) MMP/2.0 (compatible; Googlebot-Mobile/2.1; +http://www.google.com/bot.html)",
                // Googlebot-Mobile
                "Mozilla/5.0 (Windows NT 6.1; rv:6.0) Gecko/20110814 Firefox/6.0 Google (+https://developers.google.com/+/web/snippet/)", // Googlebot snippet
                "Googlebot-Image/1.0", // Googlebot-Image/1.0
                "DoCoMo/2.0 N905i(c100;TB;W24H16) (compatible; Googlebot-Mobile/2.1; +http://www.google.com/bot.html)", // Googlebot-Mobile
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.51 (KHTML, like Gecko; Google Web Preview) Chrome/12.0.742 Safari/534.51", // Google Web Preview
                "AdsBot-Google-Mobile (+http://www.google.com/mobile/adsbot.html) Mozilla (iPhone; U; CPU iPhone OS 3 0 like Mac OS X) AppleWebKit (KHTML, like Gecko) Mobile Safari",
                // AdsBot-Google-Mobile
                "Googlebot-Video/1.0", // Googlebot-Video/1.0
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.24 (KHTML, like Gecko; Google Web Preview) Chrome/11.0.696 Safari/534.24 ", // Google Web Preview
                "Mozilla/5.0 (en-us) AppleWebKit/525.13 (KHTML, like Gecko; Google Web Preview) Version/3.1 Safari/525.13", // Google Web Preview

                // Yahoo!
                "Mozilla/5.0 (compatible; Yahoo! Slurp; http://help.yahoo.com/help/us/ysearch/slurp)", // Yahoo! Slurp
                "Y!J-BRJ/YATS crawler (http://help.yahoo.co.jp/help/jp/search/indexing/indexing-15.html)", // Y!J-BRJ/YATS
                "YahooCacheSystem", // YahooCacheSystem
                "Y!J-BRO/YFSJ crawler (compatible; Mozilla 4.0; MSIE 5.5; http://help.yahoo.co.jp/help/jp/search/indexing/indexing-15.html; YahooFeedSeekerJp/2.0)", // Y!J-BRO/YFSJ
                "Y!J-BSC/1.0 crawler (http://help.yahoo.co.jp/help/jp/blog-search/)", // Y!J-BSC/1.0
                "Y!J-BRW/1.0 crawler (http://help.yahoo.co.jp/help/jp/search/indexing/indexing-15.html)", // Y!J-BRW/1.0
                "Y!J-BRI/0.0.1 crawler ( http://help.yahoo.co.jp/help/jp/search/indexing/indexing-15.html )", // Y!J-BRI/0.0.1
                "Y!J-BRJ/YATS crawler (http://listing.yahoo.co.jp/support/faq/int/other/other_001.html)", // Y!J-BRJ/YATS
                "Y!J-BSC/1.0 (http://help.yahoo.co.jp/help/jp/blog-search/)", // Y!J-BSC/1.0
                "Mozilla/5.0 (compatible; Yahoo! Slurp China; http://misc.yahoo.com.cn/help.html)", // Yahoo! Slurp China
                "Mozilla/5.0 (Yahoo-MMCrawler/4.0; mailto:vertical-crawl-support@yahoo-inc.com)", // Yahoo-MMCrawler/4.0
                "Mozilla/5.0 (compatible; Yahoo! Slurp/3.0; http://help.yahoo.com/help/us/ysearch/slurp)", // Yahoo! Slurp/3.0
                "Yahoo! Site Explorer Feed Validator http://help.yahoo.com/l/us/yahoo/search/siteexplorer/manage/", // Yahoo! Site Explorer Feed Validator

                // Bing
                "Mozilla/5.0 (compatible; bingbot/2.0; +http://www.bing.com/bingbot.htm)", // bingbot/2.0
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534+ (KHTML, like Gecko) BingPreview/1.0b", // BingPreview/1.0b
                "Mozilla/5.0 (seoanalyzer; compatible; bingbot/2.0; +http://www.bing.com/bingbot.htm)", // bingbot/2.0 seoanalyser
                "Mozilla/5.0 (compatible; bingbot/2.0; +http://www.bing.com/bingbot.htm) SitemapProbe", // bingbot SitemapProbe
                "Mozilla/5.0 (compatible; bingbot/2.0; +http://www.bing.com/bingbot.htm", // bingbot/2.0

                // Yandex
                "Mozilla/5.0 (compatible; YandexBot/3.0; +http://yandex.com/bots)", // YandexBot/3.0
                "Mozilla/5.0 (compatible; YandexImages/3.0; +http://yandex.com/bots)", // YandexImages/3.0
                "Mozilla/5.0 (compatible; YandexMedia/3.0; +http://yandex.com/bots)", // Mozilla/5.0 (compatible; YandexMedia/3.0
                "Mozilla/5.0 (compatible; YandexBot/3.0; MirrorDetector; +http://yandex.com/bots)", // YandexBot/3.0-MirrorDetector
                "Mozilla/5.0 (compatible; YandexMedia/3.0; +http://yandex.com/bots)", // YandexMedia/3.0
                "Mozilla/5.0 (compatible; YandexBlogs/0.99; robot; +http://yandex.com/bots)", // YandexBlogs/0.99
                "Mozilla/5.0 (compatible; YandexVideo/3.0; +http://yandex.com/bots)", // YandexVideo/3.0
                "Mozilla/5.0 (compatible; YandexZakladki/3.0; +http://yandex.com/bots)", // YandexZakladki/3.0
                "Mozilla/5.0 (compatible; YandexAntivirus/2.0; +http://yandex.com/bots)", // YandexAntivirus/2.0
                "Mozilla/5.0 (compatible; YandexFavicons/1.0; +http://yandex.com/bots)", // YandexFavicons/1.0
                "Mozilla/5.0 (compatible; YandexDirect/3.0; +http://yandex.com/bots)", // YandexDirect/3.0
                "Mozilla/5.0 (compatible; YandexCatalog/3.0; +http://yandex.com/bots)", // YandexCatalog/3.0
                "Mozilla/5.0 (compatible; YandexImageResizer/2.0; +http://yandex.com/bots)", // YandexImageResizer/2.0
                "Yandex/1.01.001 (compatible; Win16; I)", // Yandex/1.01.001
                "Mozilla/5.0 (compatible; YandexWebmaster/2.0; +http://yandex.com/bots)", // YandexWebmaster/2.0
                "YandexSomething/1.0" // YandexSomething/1.0
            };

            // Act
            IEnumerable<bool> results = userAgents.Select(verifier.IsBot);

            // Assert
            Assert.True(results.All(r => r));
        }
    }
}