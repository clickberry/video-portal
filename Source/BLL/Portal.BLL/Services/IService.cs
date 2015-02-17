// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portal.BLL.Services
{
    public interface IService<T>
    {
        Task<T> AddAsync(T entity);

        Task<List<T>> AddAsync(IList<T> entity);

        Task<T> GetAsync(T entity);

        Task<List<T>> GetListAsync(T entity);

        Task<T> EditAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteAsync(IList<T> entity);
    }
}