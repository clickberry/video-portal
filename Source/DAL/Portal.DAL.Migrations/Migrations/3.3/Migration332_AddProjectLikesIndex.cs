// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration332_AddProjectLikesIndex : Migration, IPortalMigration
    {
        public Migration332_AddProjectLikesIndex()
            : base("3.3.2")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> projectCollection = Database.GetCollection("Project");

            projectCollection.CreateIndex(new IndexKeysBuilder().Ascending("LikesCount"), new IndexOptionsBuilder().SetSparse(true));
        }
    }
}