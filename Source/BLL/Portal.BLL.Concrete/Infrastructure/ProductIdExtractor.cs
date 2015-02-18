// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Portal.BLL.Infrastructure;
using Portal.Domain.ProjectContext;

namespace Portal.BLL.Concrete.Infrastructure
{
    public sealed class ProductIdExtractor : IProductIdExtractor
    {
        private readonly Regex _productNameExpression = new Regex("(^[^/]+)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly Dictionary<string, ProductType> _projects = new Dictionary<string, ProductType>
        {
            { "CIC PC", ProductType.CicPc },
            { "CIC MAC", ProductType.CicMac },
            { "CIC IPAD", ProductType.CicIPad },
            { "TAGGER IPHONE", ProductType.TaggerIPhone },
            { "TAGGER ANDROID", ProductType.TaggerAndroid },
            { "IMAGESHACK YOUTUBE EXTENSION", ProductType.ImageShack },
            { "STANDALONE YOUTUBE EXTENSION", ProductType.Standalone },
            { "YOUTUBE PLAYER EXTENSION", ProductType.YoutubePlayer },
            { "DAILY MOTION EXTENSION", ProductType.DailyMotion },
            { "JW PLAYER EXTENSION", ProductType.JwPlayer }
        };

        public ProductType Get(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return ProductType.Other;
            }

            Match match = _productNameExpression.Match(userAgent);
            if (!match.Success)
            {
                return ProductType.Other;
            }

            string value = match.Groups[1].Value.ToUpperInvariant();
            if (!_projects.ContainsKey(value))
            {
                return ProductType.Other;
            }

            return _projects[value];
        }
    }
}