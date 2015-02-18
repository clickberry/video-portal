// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Driver;
using MongoRepository;
using Portal.DAL.Context;

namespace Portal.DAL.Azure.Context
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly MongoUrl _url;

        public RepositoryFactory(MongoUrl url)
        {
            _url = url;
        }

        public ITableRepository<T> Create<T>() where T : class, IEntity, new()
        {
            return new MongoTableRepository<T>(_url);
        }
    }
}