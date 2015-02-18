// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations
{
    public class Migration3212_UnsetNullEmails : Migration, IPortalMigration
    {
        public Migration3212_UnsetNullEmails()
            : base("3.2.12")
        {
        }

        public override void Update()
        {
            // Remove null Email field
            MongoCollection<BsonDocument> companyCollection = Database.GetCollection("User");
            companyCollection.Update(
                Query.And(Query.Exists("Email"), Query.EQ("Email", BsonNull.Value)),
                MongoDB.Driver.Builders.Update.Unset("Email"), UpdateFlags.Multi);
        }
    }
}