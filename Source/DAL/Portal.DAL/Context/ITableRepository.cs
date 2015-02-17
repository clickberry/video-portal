// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoRepository;

namespace Portal.DAL.Context
{
    public interface ITableRepository<T> : IRepository<T> where T : IEntity
    {
        Task<List<T>> ToListAsync();

        Task<List<T>> ToListAsync(Expression<Func<T, bool>> predicate);

        Task<List<T>> TakeAsync(int count);

        Task<List<T>> TakeAsync(Expression<Func<T, bool>> predicate, int count);

        Task<T> FirstAsync();

        Task<T> FirstAsync(Expression<Func<T, bool>> predicate);

        Task<T> FirstOrDefaultAsync();

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<T> SingleAsync();

        Task<T> SingleAsync(Expression<Func<T, bool>> predicate);

        Task<T> SingleOrDefaultAsync();

        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

        IEnumerable<T> AsEnumerable();

        IEnumerable<T> AsEnumerable(Expression<Func<T, bool>> predicate);

        Task DeleteAsync(IEnumerable<T> entities);

        Task DeleteAsync(T entity);
    }
}