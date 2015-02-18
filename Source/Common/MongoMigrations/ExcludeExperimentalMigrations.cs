// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace MongoMigrations
{
    using System.Linq;

    public class ExcludeExperimentalMigrations : MigrationFilter
    {
        public override bool Exclude(Migration migration)
        {
            if (migration == null)
            {
                return false;
            }
            return migration.GetType()
                .GetCustomAttributes(true)
                .OfType<ExperimentalAttribute>()
                .Any();
        }
    }
}