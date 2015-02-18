// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration328_AddSubscriptionHostnameIndex : Migration, IPortalMigration
    {
        public Migration328_AddSubscriptionHostnameIndex()
            : base("3.2.8")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> companyCollection = Database.GetCollection("Company");

            foreach (var company in companyCollection.FindAll())
            {
                // Hostname should be unique within Subscriptions collection
                var subscriptions = new Dictionary<string, BsonDocument>();

                foreach (var element in company["Subscriptions"].AsBsonArray)
                {
                    var subscription = element.AsBsonDocument;

                    // Check whether url is valid
                    var url = subscription["SiteUrl"].AsString;
                    Uri uri;
                    if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
                    {
                        continue;
                    }

                    // Rename SiteUrl to SiteHostname
                    var hostname = uri.Host.ToLowerInvariant();
                    subscription.Remove("SiteUrl");
                    subscription["SiteHostname"] = BsonValue.Create(hostname);

                    subscriptions[hostname] = subscription;
                }

                company["Subscriptions"] = new BsonArray(subscriptions.Values);

                companyCollection.Save(company);
            }

            // Create unique index on SiteHostname
            companyCollection.CreateIndex(new IndexKeysBuilder().Ascending("Subscriptions.SiteHostname"), new IndexOptionsBuilder().SetUnique(true).SetSparse(true));
        }
    }
}