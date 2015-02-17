// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Timers;
using MiddleEnd.Worker.Abstract;
using Portal.Domain.ProcessedVideoContext;

namespace MiddleEnd.Worker.Concrete
{
    public sealed class TaskStateManager : ITaskStateManager
    {
        private readonly ITaskChecker _taskChecker;
        private readonly ITaskKeeper _taskKeeper;
        private readonly Timer _timer = new Timer();
        private TaskProviderState _state;

        public TaskStateManager(ITaskKeeper taskKeeper, ITaskChecker taskChecker)
        {
            if (taskKeeper == null)
            {
                throw new ArgumentNullException("taskKeeper");
            }

            UpdateInterval = TimeSpan.FromSeconds(10);

            _taskKeeper = taskKeeper;
            _taskChecker = taskChecker;

            _timer.Interval = 10000;
            _timer.Elapsed += (sender, args) => Update();

            _state = TaskProviderState.Stopped;
        }

        public TimeSpan UpdateInterval
        {
            get { return TimeSpan.FromMilliseconds(_timer.Interval); }
            set { _timer.Interval = value.TotalMilliseconds; }
        }

        public void Start()
        {
            _timer.Start();
            ChangeState(TaskProviderState.Started);
        }

        public void Stop()
        {
            _timer.Stop();
            ChangeState(TaskProviderState.Stopped);
        }

        private void Update()
        {
            // Pauses timer
            _timer.Stop();

            // Check tasks
            _taskChecker.CheckTasks();

            // Updated modified entities
            _taskKeeper.Update().ContinueWith(p =>
            {
                // Resumes timer
                if (_state == TaskProviderState.Started)
                {
                    _timer.Start();
                }
            });
        }

        private void ChangeState(TaskProviderState state)
        {
            _state = state;
        }
    }
}