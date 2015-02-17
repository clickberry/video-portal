// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration337_TruncateEmailsFromNames : Migration, IPortalMigration
    {
        public Migration337_TruncateEmailsFromNames()
            : base("3.3.7")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> userCollection = Database.GetCollection("User");
            Parallel.ForEach(userCollection.FindAll(), user =>
            {
                BsonValue name = user["Name"];
                if (name == BsonNull.Value || string.IsNullOrEmpty(name.AsString))
                {
                    return;
                }

                if (!name.AsString.Contains("@"))
                {
                    return;
                }

                // replace name
                string newName = name.AsString.Substring(0, name.AsString.IndexOf("@"));
                user["Name"] = BsonValue.Create(newName);
                user["NameSort"] = BsonValue.Create(newName.ToLowerInvariant());
                userCollection.Save(user);
            });
        }
    }
}