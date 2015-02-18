// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Portal.Domain.ProcessedVideoContext.States
{
    public sealed class ReservedState : IProcessingState
    {
        private readonly List<TaskState> _previousStates;

        public ReservedState()
        {
            _previousStates = new List<TaskState>
            {
                TaskState.Waiting,
                TaskState.Cancelled,
                TaskState.Failed,
            };

            State = TaskState.Reserved;
        }

        public TaskState GetState(IProcessedEntity entity)
        {
            if (!_previousStates.Contains(entity.State))
            {
                string message = string.Format("Unable to change state from {0} to {1}", entity.State, State);
                throw new InvalidOperationException(message);
            }

            return State;
        }

        public TaskState State { get; private set; }
    }
}