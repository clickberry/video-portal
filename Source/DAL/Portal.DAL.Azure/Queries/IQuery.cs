﻿// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Driver;
using MongoRepository;

namespace Portal.DAL.Azure.Queries
{
    public interface IQuery<in T> where T : IEntity
    {
        IMongoQuery Create(T entity);
    }
}