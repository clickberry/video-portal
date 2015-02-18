// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace MongoMigrations
{
    using MongoDB.Driver;

    public abstract class Migration
    {
        public Version Version { get; protected set; }
        public string Description { get; protected set; }

        protected Migration(string version)
        {
            Version = new Version(version);
        }

        protected Migration(Version version)
        {
            Version = version;
        }

        public MongoDatabase Database { get; set; }

        public abstract void Update();
    }
}