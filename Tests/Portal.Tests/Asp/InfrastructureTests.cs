// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Asp.Infrastructure.Modules.UriModification;
using Xunit;

namespace Portal.Tests.Asp
{
    public sealed class InfrastructureTests
    {
        [Fact]
        public void CheckRedirectFromUrlWithWwwPrefix()
        {
            // Arrange
            var uriModifier = new PrefixUriModifier();
            var sourceUrls = new List<Uri>
            {
                new Uri("http://www.clickberry.tv"),
                new Uri("https://www.clickberry.tv"),
                new Uri("http://www.clickberry.tv/video"),
                new Uri("https://www.clickberry.tv/video")
            };

            var expectedUrls = new List<Uri>
            {
                new Uri("http://clickberry.tv/"),
                new Uri("https://clickberry.tv/"),
                new Uri("http://clickberry.tv/video"),
                new Uri("https://clickberry.tv/video")
            };

            // Act
            IEnumerable<Uri> destinationUrls = sourceUrls.Select(uriModifier.Process);

            // Assert
            Assert.Equal(expectedUrls, destinationUrls);
        }

        [Fact]
        public void CheckRedirectToHttps()
        {
            // Arrange
            var uriModifier = new HttpsUriModifier();
            var sourceUrls = new List<Uri>
            {
                new Uri("http://clickberry.tv/profile"),
                new Uri("https://clickberry.tv/profile"),
                new Uri("http://clickberry.tv/account"),
                new Uri("https://clickberry.tv/account"),
                new Uri("http://clickberry.tv"),
                new Uri("https://clickberry.tv"),
                new Uri("http://clickberry.tv/video"),
                new Uri("https://clickberry.tv/video")
            };

            var expectedUrls = new List<Uri>
            {
                new Uri("https://clickberry.tv/profile"),
                new Uri("https://clickberry.tv/profile"),
                new Uri("https://clickberry.tv/account"),
                new Uri("https://clickberry.tv/account"),
                new Uri("http://clickberry.tv"),
                new Uri("https://clickberry.tv"),
                new Uri("http://clickberry.tv/video"),
                new Uri("https://clickberry.tv/video")
            };

            // Act
            IEnumerable<Uri> destinationUrls = sourceUrls.Select(uriModifier.Process);

            // Assert
            Assert.Equal(expectedUrls, destinationUrls);
        }
    }
}