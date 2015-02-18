// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration312_UpdateDataSchema : Migration, IPortalMigration
    {
        public Migration312_UpdateDataSchema()
            : base("3.1.2")
        {
        }

        public override void Update()
        {
            // Removing obsolete fields
            Database.GetCollection("StorageFile").Update(Query.Null, MongoDB.Driver.Builders.Update.Unset("OwnerUserId"), UpdateFlags.Multi);
            Database.GetCollection("UserMembership").Update(Query.Null, MongoDB.Driver.Builders.Update.Unset("Comment"), UpdateFlags.Multi);

            // Normalizing emails
            MongoCollection<BsonDocument> userMembership = Database.GetCollection("UserMembership");
            MongoCursor<BsonDocument> memberships = userMembership.Find(
                Query.And(Query.EQ("IdentityProvider", "Email"),
                    Query.Exists("UserIdentifier")));

            foreach (BsonDocument membership in memberships)
            {
                // updating
                BsonValue value = membership["UserIdentifier"];
                string originalValue = value != null && value.IsString ? value.AsString : null;
                string newValue = string.IsNullOrEmpty(originalValue) ? null : originalValue.ToLowerInvariant();

                // saving
                if (originalValue == newValue)
                {
                    continue;
                }

                membership["UserIdentifier"] = newValue;

                userMembership.Save(membership);
            }

            // Adding a UserNameSort field in lower case
            MongoCollection<BsonDocument> userProfile = Database.GetCollection("UserProfile");
            MongoCursor<BsonDocument> profiles = userProfile.Find(Query.Exists("UserName"));
            foreach (BsonDocument doc in profiles)
            {
                BsonValue value = doc["UserName"];
                string originalValue = value != null && value.IsString ? value.AsString : null;
                string newValue = string.IsNullOrEmpty(originalValue) ? null : originalValue.ToLowerInvariant();

                BsonValue sortValue = doc["UserNameSort"];
                if (sortValue != null &&
                    (sortValue.IsString && sortValue.AsString == newValue ||
                     sortValue.IsBsonNull && string.IsNullOrEmpty(newValue)))
                {
                    continue;
                }

                doc["UserName"] = BsonValue.Create(originalValue) ?? BsonNull.Value;
                doc["UserNameSort"] = BsonValue.Create(newValue) ?? BsonNull.Value;

                userProfile.Save(doc);
            }
        }
    }
}