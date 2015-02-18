// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoMigrations;

namespace Portal.DAL.Migrations
{
    public class Migration3215_RemoveStatLoginIndex : Migration, IPortalMigration
    {
        public Migration3215_RemoveStatLoginIndex()
            : base("3.2.15")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> statLoginCollection = Database.GetCollection("StatUserLoginV2");

            // Drop unused index
            if (statLoginCollection.IndexExistsByName("Tick_1_EventId_1"))
            {
                statLoginCollection.DropIndexByName("Tick_1_EventId_1");
            }
        }
    }
}