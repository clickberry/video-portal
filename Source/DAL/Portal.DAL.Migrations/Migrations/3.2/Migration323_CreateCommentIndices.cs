// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration323_CreateCommentIndices : Migration, IPortalMigration
    {
        public Migration323_CreateCommentIndices()
            : base("3.2.3")
        {
        }

        public override void Update()
        {
            Database.GetCollection("Project").Update(Query.NotExists("EnableComments"), MongoDB.Driver.Builders.Update.Set("EnableComments", true), UpdateFlags.Multi);
            Database.GetCollection("Comment").CreateIndex(new IndexKeysBuilder().Ascending("ProjectId"), new IndexOptionsBuilder().SetSparse(true));
        }
    }
}