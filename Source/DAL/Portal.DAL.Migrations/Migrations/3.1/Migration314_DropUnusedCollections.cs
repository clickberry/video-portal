// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration314_DropUnusedCollections : Migration, IPortalMigration
    {
        public Migration314_DropUnusedCollections()
            : base("3.1.4")
        {
        }

        public override void Update()
        {
            var usedCollections = new HashSet<string>
            {
                "Authentication",
                "DatabaseVersion",
                "HitsCountUpdateV2",
                "HitsCountV2",
                "PasswordRecovery",
                "ProcessedScreenshot",
                "ProcessedVideo",
                "Project",
                "PushNotification",
                "SendEmail",
                "StandardReportV3",
                "StatProjectDeletionV2",
                "StatProjectStateV3",
                "StatProjectUploadingV2",
                "StatUserLoginV2",
                "StatUserRegistrationV2",
                "StatWatchingV2",
                "StorageFile",
                "StorageSpace",
                "UserMembership",
                "UserProfile",
                "UserRole",
                "VideoQueue"
            };

            IEnumerable<string> collectionsToRemove = Database.GetCollectionNames()
                .Except(usedCollections)
                .Where(p => !p.StartsWith("system."));

            foreach (string collectionName in collectionsToRemove)
            {
                Database.DropCollection(collectionName);
                Console.WriteLine("Was deleted: {0}", collectionName);
            }
        }
    }
}