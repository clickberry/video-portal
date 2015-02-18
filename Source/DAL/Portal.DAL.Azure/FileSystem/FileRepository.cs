// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoRepository;
using Portal.DAL.Entities.Table;
using Portal.DAL.FileSystem;

namespace Portal.DAL.Azure.FileSystem
{
    public class FileRepository : MongoRepository<FileEntity>, IFileRepository
    {
        public FileRepository(MongoUrl url) : base(url)
        {
        }

        public Task UpdateUserIdFromAsync(string userId, string toUserId)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<FileEntity>.EQ(entity => entity.UserId, userId);
                UpdateBuilder<FileEntity> update = Update<FileEntity>.Set(entity => entity.UserId, toUserId);

                Collection.Update(query, update, UpdateFlags.Multi);
            });
        }
    }
}