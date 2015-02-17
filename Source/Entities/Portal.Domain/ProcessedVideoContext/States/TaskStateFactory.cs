// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Portal.Domain.ProcessedVideoContext.States
{
    public static class TaskStateFactory
    {
        private static readonly Dictionary<TaskState, IProcessingState> States;

        static TaskStateFactory()
        {
            States = new Dictionary<TaskState, IProcessingState>
            {
                { TaskState.Cancelled, new CancelledState() },
                { TaskState.Completed, new CompletedState() },
                { TaskState.Corrupted, new CorruptedState() },
                { TaskState.Failed, new FailedState() },
                { TaskState.Processing, new ProcessingState() },
                { TaskState.Reserved, new ReservedState() },
                { TaskState.Waiting, new WaitingState() },
                { TaskState.Deleted, new DeletedState() }
            };
        }

        public static IProcessingState GetState(TaskState state)
        {
            if (States.ContainsKey(state))
            {
                return States[state];
            }

            throw new ArgumentOutOfRangeException("state");
        }
    }
}