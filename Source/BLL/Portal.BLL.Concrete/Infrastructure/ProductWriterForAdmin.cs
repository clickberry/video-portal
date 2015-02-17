// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BLL.Infrastructure;
using Portal.Domain.ProjectContext;

namespace Portal.BLL.Concrete.Infrastructure
{
    public class ProductWriterForAdmin : IProductWriterForAdmin
    {
        public string WriteProduct(int productId)
        {
            var product = (ProductType)productId;
            switch (product)
            {
                case ProductType.ClickberryEditor:
                    return "Editor Pro";

                case ProductType.CicMac:
                    return "CIC MAC";

                case ProductType.CicPc:
                    return "CIC PC";

                case ProductType.CicIPad:
                    return "CIC IPAD";

                case ProductType.TaggerIPhone:
                    return "Tagger IPHONE";

                case ProductType.TaggerAndroid:
                    return "Tagger Android";

                case ProductType.ImageShack:
                    return "ImageShack YouTube Extension";

                case ProductType.Standalone:
                    return "Standalone YouTube Extension";

                case ProductType.YoutubePlayer:
                    return "YouTube Player Extension";

                case ProductType.DailyMotion:
                    return "Daily Motion Extension";

                case ProductType.JwPlayer:
                    return "JW Player Extension";

                default:
                    return "Other";
            }
        }
    }
}