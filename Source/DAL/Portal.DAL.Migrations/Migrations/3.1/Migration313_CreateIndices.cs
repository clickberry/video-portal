// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration313_CreateIndices : Migration, IPortalMigration
    {
        public Migration313_CreateIndices()
            : base("3.1.3")
        {
        }

        public override void Update()
        {
            // Indices for Project colelction
            MongoCollection<BsonDocument> project = Database.GetCollection("Project");
            project.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));
            project.CreateIndex(new IndexKeysBuilder().Ascending("Created"), new IndexOptionsBuilder().SetSparse(true));
            project.CreateIndex(new IndexKeysBuilder().Ascending("VideoSource"), new IndexOptionsBuilder().SetSparse(true));
            project.CreateIndex(new IndexKeysBuilder().Ascending("NameSort"), new IndexOptionsBuilder().SetSparse(true));
            project.CreateIndex(new IndexKeysBuilder().Ascending("ProductId", "Created"), new IndexOptionsBuilder().SetSparse(true));
            project.CreateIndex(new IndexKeysBuilder().Ascending("ProductId", "Name"), new IndexOptionsBuilder().SetSparse(true));

            MongoCollection<BsonDocument> processedScreenshot = Database.GetCollection("ProcessedScreenshot");
            if (processedScreenshot.IndexExistsByName("VideoFileHash_1_TimeOffset_1"))
            {
                processedScreenshot.DropIndexByName("VideoFileHash_1_TimeOffset_1");
            }
            processedScreenshot.CreateIndex(new IndexKeysBuilder().Ascending("SourceFileId"), new IndexOptionsBuilder().SetSparse(true));

            MongoCollection<BsonDocument> processedVideo = Database.GetCollection("ProcessedVideo");
            if (processedVideo.IndexExistsByName("OriginalVideoFileHash_1_OutputFormat_1"))
            {
                processedVideo.DropIndexByName("OriginalVideoFileHash_1_OutputFormat_1");
            }
            processedVideo.CreateIndex(new IndexKeysBuilder().Ascending("SourceFileId"), new IndexOptionsBuilder().SetSparse(true));

            var videoQueueCollection = Database.GetCollection("VideoQueue");
            if (videoQueueCollection.IndexExistsByName("VideoFileHash_1"))
            {
                videoQueueCollection.DropIndexByName("VideoFileHash_1");
            }

            MongoCollection<BsonDocument> userProfile = Database.GetCollection("UserProfile");
            if (userProfile.IndexExistsByName("AppName_1_UserId_1"))
            {
                userProfile.DropIndexByName("AppName_1_UserId_1");
            }

            userProfile.CreateIndex(new IndexKeysBuilder().Ascending("UserId", "AppName"), new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            userProfile.CreateIndex(new IndexKeysBuilder().Ascending("Created"), new IndexOptionsBuilder().SetSparse(true));
            userProfile.CreateIndex(new IndexKeysBuilder().Ascending("UserNameSort"), new IndexOptionsBuilder().SetSparse(true));

            MongoCollection<BsonDocument> userMembership = Database.GetCollection("UserMembership");
            userMembership.CreateIndex(new IndexKeysBuilder().Ascending("UserIdentifier"), new IndexOptionsBuilder().SetSparse(true));
            userMembership.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));

            Database.GetCollection("HitsCountV2").CreateIndex(new IndexKeysBuilder().Ascending("ProjectId"), new IndexOptionsBuilder().SetSparse(true));
        }
    }
}