// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration3210_AddCompanySubscriptionsState : Migration, IPortalMigration
    {
        public Migration3210_AddCompanySubscriptionsState()
            : base("3.2.10")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> companyCollection = Database.GetCollection("Company");

            foreach (BsonDocument company in companyCollection.FindAll())
            {
                var subscriptions = new List<BsonDocument>();

                foreach (BsonValue element in company["Subscriptions"].AsBsonArray)
                {
                    int state = 0;
                    BsonDocument subscription = element.AsBsonDocument;

                    if (!subscription.Contains("IsDeleted"))
                    {
                        continue;
                    }

                    if (subscription["IsDeleted"].AsBoolean)
                    {
                        state = 2;
                    }

                    subscription.Remove("IsDeleted");
                    subscription.Set("State", state);

                    subscriptions.Add(subscription);
                }

                company["Subscriptions"] = new BsonArray(subscriptions);

                companyCollection.Save(company);
            }
        }
    }
}