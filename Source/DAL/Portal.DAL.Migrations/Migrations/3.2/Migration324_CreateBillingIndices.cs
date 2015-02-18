// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration324_CreateBillingIndices : Migration, IPortalMigration
    {
        public Migration324_CreateBillingIndices()
            : base("3.2.4")
        {
        }

        public override void Update()
        {
            Database.GetCollection("BalanceHistory").CreateIndex(new IndexKeysBuilder().Ascending("CompanyId"), new IndexOptionsBuilder().SetSparse(true));

            Database.GetCollection("TrackingStat").CreateIndex(new IndexKeysBuilder().Ascending("SubscriptionId", "RedirectUrl", "Date"), new IndexOptionsBuilder().SetSparse(true));
            Database.GetCollection("TrackingStat").CreateIndex(new IndexKeysBuilder().Ascending("SubscriptionId", "Date", "RedirectUrl"), new IndexOptionsBuilder().SetSparse(true));
        }
    }
}