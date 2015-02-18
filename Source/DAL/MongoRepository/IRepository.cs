// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoRepository
{
    public interface IRepository<T> where T : IEntity
    {
        IQueryable<T> Context { get; }

        MongoCollection<T> Collection { get; }

        Task<T> AddAsync(T user);

        Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities);

        Task<T> AddOrUpdateAsync(T entity);

        Task<IEnumerable<T>> AddOrUpdateAsync(IEnumerable<T> entities);

        Task<T> GetAsync(string id);

        Task<List<T>> GetAllAsync();

        Task<T> UpdateAsync(T user);

        Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities);

        Task DeleteAsync(string id);
    }
}