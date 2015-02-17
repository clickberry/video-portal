// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using MongoRepository;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.FileSystem
{
    public interface IFileRepository : IRepository<FileEntity>
    {
        Task UpdateUserIdFromAsync(string userId, string toUserId);
    }
}