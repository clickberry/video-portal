// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration330_AddProjectIndices : Migration, IPortalMigration
    {
        public Migration330_AddProjectIndices()
            : base("3.3.0")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> projectCollection = Database.GetCollection("Project");

            projectCollection.CreateIndex(new IndexKeysBuilder().Ascending("ProjectType"), new IndexOptionsBuilder().SetSparse(true));
            projectCollection.CreateIndex(new IndexKeysBuilder().Ascending("ProjectSubtype"), new IndexOptionsBuilder().SetSparse(true));
            projectCollection.CreateIndex(new IndexKeysBuilder().Ascending("VideoSource"), new IndexOptionsBuilder().SetSparse(true));
        }
    }
}