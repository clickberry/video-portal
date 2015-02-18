// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration326_UpdateProductIdForJwPlayer : Migration, IPortalMigration
    {
        public Migration326_UpdateProductIdForJwPlayer()
            : base("3.2.6")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> projectCollection = Database.GetCollection("Project");
            MongoCursor<BsonDocument> projects = Database.GetCollection("StatProjectUploadingV2").Find(Query.EQ("ProductName", "JW Player Extension"));

            Parallel.ForEach(projects, project => projectCollection.Update(Query.EQ("_id", project["ProjectId"]), MongoDB.Driver.Builders.Update.Set("ProductId", 11)));
        }
    }
}