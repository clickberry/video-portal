// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Portal.BLL.Concrete.Infrastructure
{
    public sealed class CachedEntity
    {
        public CachedEntity(TimeSpan cacheInterval)
        {
            CacheInterval = cacheInterval;
        }

        public TimeSpan CacheInterval { get; private set; }

        public DateTime Cached { get; set; }

        public bool IsCompleted { get; set; }

        public Task CachedTask { get; set; }

        public object Result { get; set; }
    }
}