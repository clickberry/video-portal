// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Portal.Domain.ProjectContext
{
    public sealed class DomainProject
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProjectAccess Access { get; set; }

        public ProjectType ProjectType { get; set; }

        public ProjectSubtype ProjectSubtype { get; set; }

        public string ScreenshotFileId { get; set; }

        public ProductType ProductType { get; set; }

        public VideoType VideoType { get; set; }

        public string VideoSource { get; set; }

        public string VideoSourceProductName { get; set; }

        public string OriginalVideoFileId { get; set; }

        public string AvsxFileId { get; set; }

        public long HitsCount { get; set; }

        public long LikesCount { get; set; }

        public long DislikesCount { get; set; }

        public long AbuseCount { get; set; }

        public bool EnableComments { get; set; }

        public List<EncodedVideo> EncodedVideos { get; set; }

        public List<EncodedScreenshot> EncodedScreenshots { get; set; }

        public ResourceState State { get; set; }

        #region Nested Types

        public sealed class EncodedScreenshot
        {
            public string ProjectId { get; set; }

            public string FileId { get; set; }

            public string ContentType { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }
        }

        public sealed class EncodedVideo
        {
            public string ProjectId { get; set; }

            public string FileId { get; set; }

            public string ContentType { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }
        }

        #endregion
    }
}