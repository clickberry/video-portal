// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.Domain.ProcessedVideoContext;

namespace MiddleEnd.Worker.Abstract
{
    public interface ITaskProvider
    {
        /// <summary>
        ///     Gets a task for a processing.
        /// </summary>
        /// <param name="types">Accepted types.</param>
        /// <returns>Manager task.</returns>
        IProcessedEntity GetNext(List<ProcessedEntityType> types);

        /// <summary>
        ///     Gets a task by identifier.
        /// </summary>
        /// <param name="id">Task identifier.</param>
        /// <returns>Manager task.</returns>
        IProcessedEntity Get(string id);
    }
}