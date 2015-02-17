// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MiddleEnd.Worker.Abstract;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProcessedVideoContext.States;

namespace MiddleEnd.Worker.Concrete
{
    /// <summary>
    ///     Task keeper.
    /// </summary>
    public class TaskProvider : ITaskProvider
    {
        private readonly object _lockObject = new object();
        private readonly ITaskKeeper _taskKeeper;

        private readonly List<TaskState> _validStates = new List<TaskState>
        {
            TaskState.Failed,
            TaskState.Waiting
        };

        public TaskProvider(ITaskKeeper taskKeeper)
        {
            if (taskKeeper == null)
            {
                throw new ArgumentNullException("taskKeeper");
            }

            _taskKeeper = taskKeeper;
        }

        public IProcessedEntity GetNext(List<ProcessedEntityType> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException("types");
            }

            // Filter free projects
            IOrderedEnumerable<IProcessedEntity> entities = _taskKeeper
                .GetTasks()
                .Where(p => types.Contains(p.EntityType) && _validStates.Contains(p.State))
                .OrderByDescending(p => p.Created);

            IProcessedEntity entity;

            // It should be atomic to prevent reservation of the same task
            lock (_lockObject)
            {
                entity = SelectNextTask(entities.ToList());

                if (entity != null)
                {
                    try
                    {
                        entity.SetState(new ReservedState());
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Failed to change task state: {0}", e);
                    }
                }
            }

            return entity;
        }

        public IProcessedEntity Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            return _taskKeeper.GetTasks().FirstOrDefault(p => p.TaskId == id);
        }

        protected virtual IProcessedEntity SelectNextTask(IList<IProcessedEntity> entities)
        {
            // 1. Screenshots
            DomainProcessedScreenshot screenshot = entities
                .OfType<DomainProcessedScreenshot>()
                .FirstOrDefault();

            if (screenshot != null)
            {
                return screenshot;
            }

            // 2. Videos by resolution
            return entities
                .OfType<DomainProcessedVideo>()
                .OrderBy(p => p.VideoParam.VideoWidth)
                .ThenBy(p => p.SourceFileId)
                .FirstOrDefault();
        }
    }
}