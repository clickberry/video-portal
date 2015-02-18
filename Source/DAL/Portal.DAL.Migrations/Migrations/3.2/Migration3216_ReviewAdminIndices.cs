// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations
{
    public class Migration3216_ReviewAdminIndices : Migration, IPortalMigration
    {
        public Migration3216_ReviewAdminIndices()
            : base("3.2.16")
        {
        }

        public override void Update()
        {
            // 1. Project

            MongoCollection<BsonDocument> projectCollection = Database.GetCollection("Project");

            // Drop unused index
            if (projectCollection.IndexExistsByName("Created_1"))
            {
                projectCollection.DropIndexByName("Created_1");
            }

            // Add indices
            projectCollection.CreateIndex(new IndexKeysBuilder().Ascending("Name"), new IndexOptionsBuilder().SetSparse(true));
            projectCollection.CreateIndex(new IndexKeysBuilder().Ascending("Created", "ProductId"), new IndexOptionsBuilder().SetSparse(true));
            projectCollection.CreateIndex(new IndexKeysBuilder().Ascending("Created", "Name"), new IndexOptionsBuilder().SetSparse(true));


            // 2. User

            MongoCollection<BsonDocument> userCollection = Database.GetCollection("User");

            // Drop unused index
            if (userCollection.IndexExistsByName("Roles_1"))
            {
                userCollection.DropIndexByName("Roles_1");
            }

            // Add indices
            userCollection.CreateIndex(new IndexKeysBuilder().Ascending("Roles", "Created"), new IndexOptionsBuilder().SetSparse(true));
            userCollection.CreateIndex(new IndexKeysBuilder().Ascending("Roles", "Name"), new IndexOptionsBuilder().SetSparse(true));
            userCollection.CreateIndex(new IndexKeysBuilder().Ascending("Roles", "ProductId"), new IndexOptionsBuilder().SetSparse(true));
        }
    }
}