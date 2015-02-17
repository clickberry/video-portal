// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain.ProcessedVideoContext;

namespace MiddleEnd.Worker.Abstract
{
    public interface ITaskKeeper
    {
        /// <summary>
        ///     Gets a current tasks.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IProcessedEntity> GetTasks();

        /// <summary>
        ///     Updates a current tasks state.
        /// </summary>
        /// <returns></returns>
        Task Update();
    }
}