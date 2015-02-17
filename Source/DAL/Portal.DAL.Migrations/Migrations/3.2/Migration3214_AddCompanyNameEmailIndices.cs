// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations
{
    public class Migration3214_AddCompanyNameEmailIndices : Migration, IPortalMigration
    {
        public Migration3214_AddCompanyNameEmailIndices()
            : base("3.2.14")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> companyCollection = Database.GetCollection("Company");

            foreach (var company in companyCollection.FindAll())
            {
                BsonValue value;
                if (company.TryGetValue("Name", out value))
                {
                    var name = value.AsString;
                    if (!string.IsNullOrEmpty(name))
                    {
                        company["NameSort"] = name.ToLowerInvariant();
                    }

                    companyCollection.Save(company);
                }
            }

            // Add text index on NameSort field
            companyCollection.CreateIndex(new IndexKeysBuilder().Text("NameSort"), new IndexOptionsBuilder().SetSparse(true));
            companyCollection.CreateIndex(new IndexKeysBuilder().Ascending("Email"), new IndexOptionsBuilder().SetSparse(true));
        }
    }
}