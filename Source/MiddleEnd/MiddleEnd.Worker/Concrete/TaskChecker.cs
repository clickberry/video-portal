// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MiddleEnd.Worker.Abstract;
using Portal.Domain.ProcessedVideoContext;
using Portal.Domain.ProcessedVideoContext.States;

namespace MiddleEnd.Worker.Concrete
{
    public sealed class TaskChecker : ITaskChecker
    {
        private readonly List<TaskState> _completedStates = new List<TaskState>
        {
            TaskState.Completed,
            TaskState.Corrupted,
            TaskState.Deleted
        };

        private readonly List<TaskState> _failedStates = new List<TaskState>
        {
            TaskState.Failed
        };

        private readonly List<TaskState> _processingStates = new List<TaskState>
        {
            TaskState.Reserved,
            TaskState.Processing
        };

        private readonly ITaskKeeper _taskKeeper;


        public TaskChecker(ITaskKeeper taskKeeper)
        {
            _taskKeeper = taskKeeper;
            TaskProcessingTimeout = TimeSpan.FromMinutes(3);
        }

        /// <summary>
        ///     Gets a value indicating timeout for task processing.
        /// </summary>
        public TimeSpan TaskProcessingTimeout { get; set; }

        /// <summary>
        ///     Gets a value indicating max processing attempts count.
        /// </summary>
        public int MaxAttemptsCount
        {
            get { return 2; }
        }

        /// <summary>
        ///     Checks current tasks.
        /// </summary>
        public void CheckTasks()
        {
            // Check hanged tasks
            List<IProcessedEntity> hangedTasks = _taskKeeper
                .GetTasks()
                .Where(p => !_completedStates.Contains(p.State) &&
                            _processingStates.Contains(p.State) &&
                            DateTime.UtcNow.Subtract(p.Modified) > TaskProcessingTimeout)
                .ToList();

            foreach (IProcessedEntity entity in hangedTasks)
            {
                Trace.TraceInformation("Task {0} hanged up while video {1} processing. Current state: {2}, Attempts: {3}, DestinationFileId: {4}.",
                    entity.TaskId,
                    entity.SourceFileId,
                    entity.State,
                    entity.AttemptsCount,
                    entity.DestinationFileId ?? "null");

                entity.AttemptsCount++;

                try
                {
                    if (entity.State == TaskState.Reserved)
                    {
                        // Nobody wants to process this entity
                        entity.SetState(new WaitingState());
                    }
                    else
                    {
                        // Somebody hanged up while entity processing
                        entity.SetState(new FailedState());
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError("Failed to change task state: {0}", e);
                }
            }

            // Check failed tasks
            List<IProcessedEntity> corruptedTasks = _taskKeeper
                .GetTasks()
                .Where(p => (p.AttemptsCount >= MaxAttemptsCount && !_completedStates.Contains(p.State))).ToList();

            foreach (IProcessedEntity corruptedTask in corruptedTasks)
            {
                try
                {
                    corruptedTask.SetState(new CorruptedState());
                }
                catch (Exception e)
                {
                    Trace.TraceError("Failed to change task state: {0}", e);
                }
            }
        }

        /// <summary>
        ///     Checks processed task.
        /// </summary>
        /// <param name="task">Task entity.</param>
        public void CheckTask(IProcessedEntity task)
        {
            if (!_failedStates.Contains(task.State))
            {
                return;
            }

            task.AttemptsCount++;

            CheckTasks();
        }
    }
}