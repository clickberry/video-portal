// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Web;
using Asp.Infrastructure.Extensions;
using Configuration;
using Moq;
using Xunit;

namespace Portal.Tests.Asp
{
    public sealed class CdnResourcesTests
    {
        [Fact]
        public void TestUrlReplacementWithTilda()
        {
            // Arrange
            var moq = new Mock<IPortalFrontendSettings>();
            const string cdn = "clbr.tv";
            const string resource = "~/cdn/pic/banners/pic-1.png";
            moq.Setup(p => p.CdnUri).Returns(cdn);
            CdnResources.SetConfigurationProvider(moq.Object);


            // Act
            string result = CdnResources.RenderUrl(resource);

            // Assert
            var expected = string.Format("//{0}{1}?v=", cdn, resource.Substring(5));
            Assert.True(result.StartsWith(expected));
        }

        [Fact]
        public void TestUrlReplacementWithoutTilda()
        {
            // Arrange
            var moq = new Mock<IPortalFrontendSettings>();
            const string cdn = "clbr.tv";
            const string resource = "/cdn/pic/banners/pic-1.png";
            moq.Setup(p => p.CdnUri).Returns(cdn);
            CdnResources.SetConfigurationProvider(moq.Object);


            // Act
            string result = CdnResources.RenderUrl(resource);

            // Assert
            var expected = string.Format("//{0}{1}?v=", cdn, resource.Substring(4));
            Assert.True(result.StartsWith(expected));
        }
    }
}