// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoMigrations;

namespace Portal.DAL.Migrations.Migrations
{
    public class Migration335_FileSystem : Migration, IPortalMigration
    {
        public Migration335_FileSystem()
            : base("3.3.5")
        {
        }

        public override void Update()
        {
            MongoCollection<BsonDocument> storageFileCollection = Database.GetCollection("StorageFile");
            MongoCollection<BsonDocument> storageSpaceCollection = Database.GetCollection("StorageSpace");
            MongoCollection<BsonDocument> filesCollection = Database.GetCollection("File");

            var newFiles = new ConcurrentBag<BsonDocument>();

            Parallel.ForEach(
                storageFileCollection.FindAll(),
                file =>
                {
                    BsonDocument space = storageSpaceCollection.FindOne(Query.And(Query.EQ("UserId", file["UserId"]), Query.EQ("FileId", file["FileId"])));
                    var newFile = new BsonDocument(new Dictionary<string, object>
                    {
                        { "_id", file["FileId"] },
                        { "UserId", file["UserId"] },
                        { "Name", file["FileName"] },
                        { "Length", file["FileLength"] },
                        { "Created", file["Created"] },
                        { "Modified", file["Modified"] },
                        { "ContentType", file["ContentType"] },
                        { "IsArtifact", space != null ? space["IsArtifact"] : BsonValue.Create(true) },
                    });
                    newFiles.Add(newFile);
                });

            // Insert new documents
            if (newFiles.Count > 0)
            {
                filesCollection.InsertBatch(newFiles);
            }

            // Create indices
            filesCollection.CreateIndex(new IndexKeysBuilder().Ascending("UserId"), new IndexOptionsBuilder().SetSparse(true));
        }
    }
}