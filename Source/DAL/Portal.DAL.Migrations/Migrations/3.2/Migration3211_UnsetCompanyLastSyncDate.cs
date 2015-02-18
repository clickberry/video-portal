// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations
{
    public class Migration3211_UnsetCompanyLastSyncDate : Migration, IPortalMigration
    {
        public Migration3211_UnsetCompanyLastSyncDate()
            : base("3.2.11")
        {
        }

        public override void Update()
        {
            // Remove LastSyncDate field
            MongoCollection<BsonDocument> companyCollection = Database.GetCollection("Company");
            companyCollection.Update(Query.Null, MongoDB.Driver.Builders.Update.Unset("LastSyncDate"), UpdateFlags.Multi);
        }
    }
}