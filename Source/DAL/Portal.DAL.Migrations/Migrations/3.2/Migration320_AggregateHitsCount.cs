// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration320_AggregateHitsCount : Migration, IPortalMigration
    {
        public Migration320_AggregateHitsCount()
            : base("3.2.0")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> hitsCountCollection = Database.GetCollection("HitsCountV2");
            hitsCountCollection.CreateIndex(new IndexKeysBuilder().Ascending("ProjectId"), new IndexOptionsBuilder().SetSparse(true));

            MongoCollection<BsonDocument> projectCollection = Database.GetCollection("Project");
            MongoCursor<BsonDocument> projects = projectCollection.FindAll();

            Parallel.ForEach(projects, project =>
            {
                long hitsCount = hitsCountCollection.Find(Query.EQ("ProjectId", project["_id"])).Count();

                project["HitsCount"] = hitsCount;

                projectCollection.Save(project);
            });

            projectCollection.CreateIndex(new IndexKeysBuilder().Ascending("HitsCount"), new IndexOptionsBuilder().SetSparse(true));
        }
    }
}