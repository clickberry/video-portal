// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations
{
    public class Migration341_AddFollowersIndex : Migration, IPortalMigration
    {
        public Migration341_AddFollowersIndex()
            : base("3.4.1")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> companyCollection = Database.GetCollection("User");
            companyCollection.CreateIndex(new IndexKeysBuilder().Ascending("Followers._id"), new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
        }
    }
}