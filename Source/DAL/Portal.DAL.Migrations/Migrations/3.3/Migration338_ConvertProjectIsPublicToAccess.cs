// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration338_ConvertProjectIsPublicToAccess : Migration, IPortalMigration
    {
        public Migration338_ConvertProjectIsPublicToAccess()
            : base("3.3.8")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> projectCollection = Database.GetCollection("Project");
            Parallel.ForEach(projectCollection.FindAll(), project =>
            {
                // Already processed value
                if (project.Contains("Access"))
                {
                    return;
                }

                // Convert boolean IsPublic value into Access enumeration
                BsonValue isPublic = project["IsPublic"];
                BsonValue access;

                // Checks whether project was hidden
                if (isPublic != BsonNull.Value && !isPublic.AsBoolean)
                {
                    access = BsonValue.Create(1);
                }
                else
                {
                    access = BsonValue.Create(0);
                }

                // replace name
                project["Access"] = access;
                projectCollection.Save(project);
            });

            projectCollection.Update(Query.Null, MongoDB.Driver.Builders.Update.Unset("IsPublic"), UpdateFlags.Multi);
        }
    }
}