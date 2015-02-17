// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration321_AggregateUserData : Migration, IPortalMigration
    {
        public Migration321_AggregateUserData()
            : base("3.2.1")
        {
        }

        private string GetString(BsonValue value)
        {
            return value.IsString ? value.AsString : null;
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> userProfileCollection = Database.GetCollection("UserProfile");
            MongoCursor<BsonDocument> userProfiles = userProfileCollection.FindAll();

            MongoCollection<BsonDocument> authenticationCollection = Database.GetCollection("Authentication");
            MongoCollection<BsonDocument> membershipCollection = Database.GetCollection("UserMembership");
            MongoCollection<BsonDocument> roleCollection = Database.GetCollection("UserRole");
            MongoCollection<BsonDocument> userCollection = Database.GetCollection("User");
            MongoCollection<BsonDocument> passwordRecoveryCollection = Database.GetCollection("PasswordRecovery");
            MongoCollection<BsonDocument> processedScreenshotCollection = Database.GetCollection("ProcessedScreenshot");
            MongoCollection<BsonDocument> processedVideoCollection = Database.GetCollection("ProcessedVideo");
            MongoCollection<BsonDocument> projectCollection = Database.GetCollection("Project");
            MongoCollection<BsonDocument> pushNotificationCollection = Database.GetCollection("PushNotification");
            MongoCollection<BsonDocument> sendEmailCollection = Database.GetCollection("SendEmail");
            MongoCollection<BsonDocument> statProjectDeletionCollection = Database.GetCollection("StatProjectDeletionV2");
            MongoCollection<BsonDocument> statProjectUploadingCollection = Database.GetCollection("StatProjectUploadingV2");
            MongoCollection<BsonDocument> statUserLoginCollection = Database.GetCollection("StatUserLoginV2");
            MongoCollection<BsonDocument> statUserRegistrationCollection = Database.GetCollection("StatUserRegistrationV2");
            MongoCollection<BsonDocument> statWatchingCollection = Database.GetCollection("StatWatchingV2");
            MongoCollection<BsonDocument> storageFileCollection = Database.GetCollection("StorageFile");
            MongoCollection<BsonDocument> storageSpaceCollection = Database.GetCollection("StorageSpace");

            // Create team indexes
            Console.WriteLine("Creating temp indices...");

            roleCollection.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));
            statWatchingCollection.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));
            statProjectUploadingCollection.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));
            statUserLoginCollection.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));
            statProjectDeletionCollection.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));
            statUserRegistrationCollection.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));
            processedVideoCollection.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));
            processedScreenshotCollection.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));
            storageFileCollection.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));

            // Create unique index by e-mail
            userCollection.CreateIndex(new IndexKeysBuilder().Ascending("Email"), new IndexOptionsBuilder().SetSparse(true).SetUnique(true));

            Console.WriteLine("Aggregating user data ...");

            Parallel.ForEach(userProfiles, profile =>
            {
                // Skip registrations via private domain name
                if (profile["AppName"].AsString.EndsWith(".cloudapp.net"))
                {
                    return;
                }

                // Copy from UserProfile
                string id = profile["UserId"].AsString;
                var user = new BsonDocument
                {
                    { "_id", id },
                    { "AppName", profile["AppName"] },
                    { "Created", profile.GetValue("Created", DateTime.UtcNow) },
                    { "Modified", profile.GetValue("Modified", DateTime.UtcNow) },
                    { "Blocked", profile.GetValue("Blocked", DateTime.UtcNow) },
                    { "UserName", profile.GetValue("UserName", BsonNull.Value) },
                    { "UserNameSort", profile.GetValue("UserNameSort", BsonNull.Value) },
                    { "MaximumStorageSpace", profile.GetValue("MaximumStorageSpace", BsonValue.Create(1024*1024*1024)) },
                    { "Country", profile.GetValue("Country", BsonNull.Value) },
                    { "City", profile.GetValue("City", BsonNull.Value) },
                    { "Timezone", profile.GetValue("Timezone", "UTC") },
                    { "IsBlocked", profile.GetValue("IsBlocked", false) },
                    { "ProductId", profile.GetValue("ProductId", 0) },
                };

                // Add Memberships
                string email = null;
                string password = null;
                string passwordSalt = null;
                var userMemberships = new List<BsonDocument>();

                MongoCursor<BsonDocument> memberships = membershipCollection.Find(Query.EQ("UserId", id));

                foreach (BsonDocument membership in memberships)
                {
                    if (membership["IdentityProvider"].AsString == "Email")
                    {
                        email = email ?? GetString(membership.GetValue("UserIdentifier", BsonNull.Value));
                        password = password ?? GetString(membership.GetValue("Password", BsonNull.Value));
                        passwordSalt = passwordSalt ?? GetString(membership.GetValue("PasswordSalt", BsonNull.Value));
                    }
                    else
                    {
                        userMemberships.Add(new BsonDocument
                        {
                            { "IdentityProvider", membership["IdentityProvider"] },
                            { "UserIdentifier", membership["UserIdentifier"] },
                        });
                    }
                }

                // Override e-mail if it was found
                BsonDocument authentication = authenticationCollection.Find(Query.EQ("UserId", id)).FirstOrDefault();
                if (authentication != null)
                {
                    email = GetString(authentication.GetValue("UserEmail", BsonNull.Value)) ?? email;
                }

                user["Memberships"] = new BsonArray(userMemberships);

                if (!string.IsNullOrEmpty(email))
                {
                    user["Email"] = BsonValue.Create(email);
                }

                if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(passwordSalt))
                {
                    user["Password"] = BsonValue.Create(password);
                    user["PasswordSalt"] = BsonValue.Create(passwordSalt);
                }

                // Add Roles
                MongoCursor<BsonDocument> roles = roleCollection.Find(Query.EQ("UserId", id));
                var userRoles = new HashSet<string>(roles.Select(role => role["RoleName"].AsString)) { "User" };

                user["Roles"] = new BsonArray(userRoles);

                // Try to find other user by e-mail
                if (string.IsNullOrEmpty(email))
                {
                    try
                    {
                        userCollection.Insert(user);
                    }
                    catch (MongoDuplicateKeyException)
                    {
                        // Concurrent insert with retry
                    }

                    return;
                }

                BsonDocument existingUser = userCollection.FindOne(Query.EQ("Email", email));
                if (existingUser == null)
                {
                    try
                    {
                        userCollection.Insert(user);
                    }
                    catch (MongoDuplicateKeyException)
                    {
                    }

                    return;
                }

                // Merge user accounts
                var existingId = existingUser["_id"].AsString;

                // User name
                if (string.IsNullOrEmpty(GetString(existingUser.GetValue("UserName", BsonNull.Value))))
                {
                    existingUser["UserName"] = user["UserName"];
                    existingUser["UserNameSort"] = user["UserNameSort"];
                }

                // Country
                if (string.IsNullOrEmpty(GetString(existingUser.GetValue("Country", BsonNull.Value))))
                {
                    existingUser["Country"] = user["Country"];
                }

                // City
                if (string.IsNullOrEmpty(GetString(existingUser.GetValue("City", BsonNull.Value))))
                {
                    existingUser["City"] = user["City"];
                }

                // Memberships
                existingUser["Memberships"] = existingUser["Memberships"].AsBsonArray.AddRange(userMemberships);

                // Roles
                foreach (BsonValue role in existingUser["Roles"].AsBsonArray)
                {
                    userRoles.Add(role.AsString);
                }

                user["Roles"] = new BsonArray(userRoles);

                // Passwords
                if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(passwordSalt) &&
                    (string.IsNullOrEmpty(GetString(existingUser.GetValue("Password", BsonNull.Value))) ||
                     string.IsNullOrEmpty(GetString(existingUser.GetValue("PasswordSalt", BsonNull.Value)))))
                {
                    existingUser["Password"] = BsonValue.Create(password);
                    existingUser["PasswordSalt"] = BsonValue.Create(passwordSalt);
                }

                // Change data ownership
                passwordRecoveryCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                processedScreenshotCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                processedVideoCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                projectCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                pushNotificationCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                sendEmailCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                statProjectDeletionCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                statProjectUploadingCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                statUserLoginCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                statUserRegistrationCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                statWatchingCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                storageFileCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);
                storageSpaceCollection.Update(Query.EQ("UserId", id), MongoDB.Driver.Builders.Update.Set("UserId", existingId), UpdateFlags.Multi);

                // Save user changes
                userCollection.Save(existingUser);
            });

            // Remove temp indexes
            Console.WriteLine("Dropping temp indices...");

            statWatchingCollection.DropIndex(new IndexKeysBuilder().Ascending("UserId"));
            statProjectUploadingCollection.DropIndex(new IndexKeysBuilder().Ascending("UserId"));
            statUserLoginCollection.DropIndex(new IndexKeysBuilder().Ascending("UserId"));
            statProjectDeletionCollection.DropIndex(new IndexKeysBuilder().Ascending("UserId"));
            statUserRegistrationCollection.DropIndex(new IndexKeysBuilder().Ascending("UserId"));
            processedVideoCollection.DropIndex(new IndexKeysBuilder().Ascending("UserId"));
            processedScreenshotCollection.DropIndex(new IndexKeysBuilder().Ascending("UserId"));
            storageFileCollection.DropIndex(new IndexKeysBuilder().Ascending("UserId"));

            // Create indices
            userCollection.CreateIndex(new IndexKeysBuilder().Ascending("Created"), new IndexOptionsBuilder().SetSparse(true));
            userCollection.CreateIndex(new IndexKeysBuilder().Ascending("UserNameSort"), new IndexOptionsBuilder().SetSparse(true));
            userCollection.CreateIndex(new IndexKeysBuilder().Ascending("Roles"), new IndexOptionsBuilder().SetSparse(true));
            userCollection.CreateIndex(
                new IndexKeysBuilder().Ascending("Memberships.IdentityProvider", "Memberships.UserIdentifier"),
                new IndexOptionsBuilder().SetSparse(true).SetUnique(true));
        }
    }
}