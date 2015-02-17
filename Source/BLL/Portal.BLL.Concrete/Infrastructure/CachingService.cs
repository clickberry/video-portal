// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portal.BLL.Concrete.Infrastructure
{
    public abstract class CachingService
    {
        private static readonly ConcurrentDictionary<string, CachedEntity> Tasks;

        static CachingService()
        {
            Tasks = new ConcurrentDictionary<string, CachedEntity>();
        }

        protected static Task<List<TEntity>> GetOrAddEntities<TEntity>(string name, TimeSpan cacheInterval, Func<Task> getTask)
        {
            CachedEntity item = Tasks.GetOrAdd(name, s => new CachedEntity(cacheInterval)
            {
                Cached = DateTime.UtcNow,
                CachedTask = getTask()
            });

            if (DateTime.UtcNow.Subtract(item.Cached) > item.CacheInterval)
            {
                Tasks[name] = new CachedEntity(cacheInterval)
                {
                    Cached = DateTime.UtcNow,
                    CachedTask = getTask()
                };
            }

            CachedEntity entity = Tasks[name];

            var tcs = new TaskCompletionSource<List<TEntity>>();

            if (entity.IsCompleted)
            {
                tcs.SetResult((List<TEntity>)entity.Result);
            }
            else
            {
                entity.CachedTask.ContinueWith(task =>
                {
                    entity.CachedTask = null;
                    entity.Result = null;

                    if (task.IsFaulted || task.IsCanceled)
                    {
                        tcs.SetException(task.Exception ?? new Exception());
                    }
                    else
                    {
                        tcs.SetResult(((Task<List<TEntity>>)task).Result);
                    }
                });
            }

            return tcs.Task;
        }
    }
}