// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration331_DropUnusedCollections : Migration, IPortalMigration
    {
        public Migration331_DropUnusedCollections()
            : base("3.3.1")
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
                "UserProfile"
            };

            foreach (string collectionName in dropCollections)
            {
                Database.DropCollection(collectionName);
            }
        }
    }
}