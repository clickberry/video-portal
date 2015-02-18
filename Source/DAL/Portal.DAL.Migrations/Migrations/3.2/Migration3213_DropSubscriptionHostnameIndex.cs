// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations
{
    public class Migration3213_DropSubscriptionHostnameIndex : Migration, IPortalMigration
    {
        public Migration3213_DropSubscriptionHostnameIndex()
            : base("3.2.13")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> companyCollection = Database.GetCollection("Company");

            // Drop unique index on SiteHostname field
            companyCollection.DropIndex(new IndexKeysBuilder().Ascending("Subscriptions.SiteHostname"));
        }
    }
}