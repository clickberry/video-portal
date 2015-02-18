// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoRepository;

namespace Portal.DAL.Context
{
    public interface IRepositoryFactory
    {
        ITableRepository<T> Create<T>() where T : class, IEntity, new();
    }
}