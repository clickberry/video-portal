// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Driver;
using MongoMigrations;
using Portal.DAL.Migrations;

namespace Configuration.Azure.Concrete
{
    /// <summary>
    ///     Checks whether we use the latest version of mongo database.
    /// </summary>
    public class MongoMigrationInitializer : IInitializable
    {
        private readonly bool _automigration;
        private readonly MongoUrl _url;

        public MongoMigrationInitializer(MongoUrl url, bool automigration = false)
        {
            _url = url;
            _automigration = automigration;
        }

        public void Initialize()
        {
            var runner = new MigrationRunner(_url);

            // migrations are ordered by version
            runner.MigrationLocator.LookForMigrationsInAssemblyOfType<IPortalMigration>();

            if (_automigration)
            {
                // updating database
                runner.UpdateToLatest();
            }

            runner.DatabaseStatus.ThrowIfNotLatestVersion();
        }
    }
}