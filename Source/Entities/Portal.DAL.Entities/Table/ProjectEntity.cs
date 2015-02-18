// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("Project")]
    [BsonIgnoreExtraElements]
    public sealed class ProjectEntity : IEntity
    {
        public ProjectEntity()
        {
            EncodedScreenshots = new EncodedScreenshot[] { };
            EncodedVideos = new EncodedVideo[] { };
        }

        [BsonId(IdGenerator = typeof (StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public string Name { get; set; }

        public string NameSort { get; set; }

        public string Description { get; set; }

        public int Access { get; set; }

        /// <summary>
        ///     Gets or sets a project type.
        /// </summary>
        public int ProjectType { get; set; }

        public int ProjectSubtype { get; set; }

        /// <summary>
        ///     Gets or sets a file id for custom uploaded screenshot.
        /// </summary>
        public string ScreenshotFileId { get; set; }

        /// <summary>
        ///     Gets or sets a generator product identifier.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///     Gets or sets a video type.
        /// </summary>
        public int VideoType { get; set; }

        /// <summary>
        ///     Gets or sets an external video Uri (if video type is 1).
        /// </summary>
        public string VideoSource { get; set; }

        /// <summary>
        ///     Gets or sets an external video product name (if video type is 1).
        /// </summary>
        public string VideoSourceProductName { get; set; }

        /// <summary>
        ///     Gets or sets a file id for original uploaded video.
        /// </summary>
        public string OriginalVideoFileId { get; set; }

        public string AvsxFileId { get; set; }

        public long HitsCount { get; set; }

        public long LikesCount { get; set; }

        public long DislikesCount { get; set; }

        public long AbuseCount { get; set; }

        public bool EnableComments { get; set; }

        public EncodedVideo[] EncodedVideos { get; set; }

        public EncodedScreenshot[] EncodedScreenshots { get; set; }

        public int State { get; set; }

        #region Nested Entities

        public sealed class EncodedScreenshot
        {
            public string FileId { get; set; }

            public string ContentType { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }
        }

        public sealed class EncodedVideo
        {
            public string FileId { get; set; }

            public string ContentType { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }
        }

        #endregion
    }
}