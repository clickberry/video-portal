// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace MongoMigrations
{
    public abstract class MigrationFilter
    {
        public abstract bool Exclude(Migration migration);
    }
}