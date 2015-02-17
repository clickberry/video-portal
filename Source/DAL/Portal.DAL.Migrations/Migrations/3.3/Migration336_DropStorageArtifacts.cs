// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration336_DropStorageArtifacts : Migration, IPortalMigration
    {
        public Migration336_DropStorageArtifacts()
            : base("3.3.6")
        {
        }

        public override void Update()
        {
            Database.GetCollection("StorageFile").Drop();
            Database.GetCollection("StorageSpace").Drop();
        }
    }
}