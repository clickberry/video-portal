// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration325_DropUnusedCollections : Migration, IPortalMigration
    {
        public Migration325_DropUnusedCollections()
            : base("3.2.5")
        {
        }

        public override void Update()
        {
            var dropCollections = new List<string>
            {
                "Authentication",
                "HitsCountV2",
                "HitsCountUpdateV2",
                "UserMembership",
                "UserProfile",
                "UserRole"
            };

            foreach (string collectionName in dropCollections)
            {
                Database.DropCollection(collectionName);
            }
        }
    }
}