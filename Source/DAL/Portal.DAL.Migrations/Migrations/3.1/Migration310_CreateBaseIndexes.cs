// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration310_CreateBaseIndexes : Migration, IPortalMigration
    {
        public Migration310_CreateBaseIndexes()
            : base("3.1.0")
        {
        }

        public override void Update()
        {
            Database.GetCollection("Authentication").CreateIndex(
                new IndexKeysBuilder().Ascending("UserId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("ExternalVideo").CreateIndex(
                new IndexKeysBuilder().Ascending("ProjectId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("HitsCountV2").CreateIndex(
                new IndexKeysBuilder().Ascending("ProjectId", "Tick"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("HitsCountUpdateV2").CreateIndex(
                new IndexKeysBuilder().Ascending("ProjectId", "Tick"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("PasswordRecovery").CreateIndex(
                new IndexKeysBuilder().Ascending("UserId", "LinkData"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("ProcessedScreenshot").CreateIndex(
                new IndexKeysBuilder().Ascending("VideoFileHash", "TimeOffset"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("ProcessedVideo").CreateIndex(
                new IndexKeysBuilder().Ascending("OriginalVideoFileHash", "OutputFormat"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("ProjectAvsx").CreateIndex(
                new IndexKeysBuilder().Ascending("ProjectId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("ProjectProcessedScreenshot").CreateIndex(
                new IndexKeysBuilder().Ascending("ProjectId", "FileId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("ProjectProcessedVideo").CreateIndex(
                new IndexKeysBuilder().Ascending("ProjectId", "FileId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("ProjectScreenshot").CreateIndex(
                new IndexKeysBuilder().Ascending("UserId", "ProjectId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("ProjectVideo").CreateIndex(
                new IndexKeysBuilder().Ascending("FileHash", "ProjectId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("PushNotification").CreateIndex(
                new IndexKeysBuilder().Ascending("ProjectId", "Id"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("SendEmail").CreateIndex(
                new IndexKeysBuilder().Ascending("UserId", "Id"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("StandardReportV3").CreateIndex(
                new IndexKeysBuilder().Ascending("Tick", "Interval"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("StatProjectDeletionV2").CreateIndex(
                new IndexKeysBuilder().Ascending("Tick", "EventId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("StatProjectStateV3").CreateIndex(
                new IndexKeysBuilder().Ascending("ProjectId", "ActionType"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("StatProjectUploadingV2").CreateIndex(
                new IndexKeysBuilder().Ascending("Tick", "EventId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("StatUserLoginV2").CreateIndex(
                new IndexKeysBuilder().Ascending("Tick", "EventId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("StatUserRegistrationV2").CreateIndex(
                new IndexKeysBuilder().Ascending("Tick", "EventId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("StatWatchingV2").CreateIndex(
                new IndexKeysBuilder().Ascending("Tick", "EventId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("StorageFile").CreateIndex(
                new IndexKeysBuilder().Ascending("FileId", "EntryId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("StorageSpace").CreateIndex(
                new IndexKeysBuilder().Ascending("UserId", "FileId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("UniqueVideo").CreateIndex(
                new IndexKeysBuilder().Ascending("VideoFileHash"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("UserMembership").CreateIndex(
                new IndexKeysBuilder().Ascending("ApplicationName", "Identity"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("UserProfile").CreateIndex(
                new IndexKeysBuilder().Ascending("AppName", "UserId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("UserProject").CreateIndex(
                new IndexKeysBuilder().Ascending("UserId", "ProjectId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("UserRole").CreateIndex(
                new IndexKeysBuilder().Ascending("RoleName", "UserId"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
            Database.GetCollection("VideoQueue").CreateIndex(
                new IndexKeysBuilder().Ascending("VideoFileHash"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
        }
    }
}