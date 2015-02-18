// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wrappers
{
    public class TaskWrapper
    {
        private Task _task;

        public TaskWrapper(Action action)
        {
            _task = new Task(action);
        }

        public TaskWrapper(Action<object> action, object state)
        {
            _task = new Task(action, state);
        }

        public TaskWrapper(Action action, CancellationToken cancellationToken)
        {
            _task = new Task(action, cancellationToken);
        }

        public TaskWrapper(Action action, TaskCreationOptions creationOptions)
        {
            _task = new Task(action, creationOptions);
        }

        public TaskWrapper(Action<object> action, object state, CancellationToken cancellationToken)
        {
            _task = new Task(action, state, cancellationToken);
        }

        public TaskWrapper(Action<object> action, object state, TaskCreationOptions creationOptions)
        {
            _task = new Task(action, state, creationOptions);
        }

        public TaskWrapper(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
        {
            _task = new Task(action, cancellationToken, creationOptions);
        }

        public TaskWrapper(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
        {
            _task = new Task(action, state, cancellationToken, creationOptions);
        }

        public virtual object AsyncState
        {
            get { throw new NotImplementedException(); }
        }

        public virtual TaskCreationOptions CreationOptions
        {
            get { throw new NotImplementedException(); }
        }

        public virtual AggregateException Exception
        {
            get { return _task.Exception; }
        }

        public virtual int Id
        {
            get { throw new NotImplementedException(); }
        }

        public virtual bool IsCanceled
        {
            get { return _task.IsCanceled; }
        }

        public virtual bool IsCompleted
        {
            get { return _task.IsCompleted; }
        }

        public virtual bool IsFaulted
        {
            get { return _task.IsFaulted; }
        }

        public virtual TaskStatus Status
        {
            get { return _task.Status; }
        }

        /// <summary>
        ///     For use internal wrappers only.
        /// </summary>
        public Task Task
        {
            get { return _task; }
            set { _task = value; }
        }

        public virtual TaskWrapper ContinueWith(Action<Task> continuationAction)
        {
            _task.ContinueWith(continuationAction);
            return this;
        }

        public virtual Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction)
        {
            return _task.ContinueWith(continuationFunction);
        }

        public virtual Task ContinueWith(Action<Task> continuationAction, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWith(Action<Task> continuationAction, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWith(Action<Task> continuationAction, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWith(Action<Task> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions,
            TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        public virtual void RunSynchronously()
        {
            throw new NotImplementedException();
        }

        public virtual void RunSynchronously(TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual void Start()
        {
            throw new NotImplementedException();
        }

        public virtual void Start(TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual void Wait()
        {
            throw new NotImplementedException();
        }

        public virtual void Wait(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual bool Wait(int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }

        public virtual bool Wait(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public virtual bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}