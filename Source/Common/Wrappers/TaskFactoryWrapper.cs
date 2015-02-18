// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wrappers
{
    public class TaskFactoryWrapper
    {
        private TaskFactory _taskFactory;

        public TaskFactoryWrapper()
        {
            _taskFactory = new TaskFactory();
        }

        public TaskFactoryWrapper(CancellationToken cancellationToken)
        {
            _taskFactory = new TaskFactory(cancellationToken);
        }

        public TaskFactoryWrapper(TaskScheduler scheduler)
        {
            _taskFactory = new TaskFactory(scheduler);
        }

        public TaskFactoryWrapper(TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions)
        {
            _taskFactory = new TaskFactory(creationOptions, continuationOptions);
        }

        public TaskFactoryWrapper(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
        {
            _taskFactory = new TaskFactory(cancellationToken, creationOptions, continuationOptions, scheduler);
        }

        public virtual CancellationToken CancellationToken
        {
            get { throw new NotImplementedException(); }
        }

        public virtual TaskContinuationOptions ContinuationOptions
        {
            get { throw new NotImplementedException(); }
        }

        public virtual TaskCreationOptions CreationOptions
        {
            get { throw new NotImplementedException(); }
        }

        public virtual TaskScheduler Scheduler
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     For use internal wrappers only.
        /// </summary>
        public TaskFactory TaskFactory
        {
            get { return _taskFactory; }
            set { _taskFactory = value; }
        }

        public virtual Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction,
            TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, CancellationToken cancellationToken,
            TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction,
            CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions,
            TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction,
            TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, CancellationToken cancellationToken,
            TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction,
            CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions,
            TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod)
        {
            throw new NotImplementedException();
        }

        public virtual Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state)
        {
            throw new NotImplementedException();
        }

        public virtual Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state)
        {
            throw new NotImplementedException();
        }

        public virtual Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state,
            TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2,
            object state)
        {
            throw new NotImplementedException();
        }

        public virtual Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state,
            TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2,
            object state, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2,
            TArg3 arg3, object state)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
        {
            throw new NotImplementedException();
        }

        public virtual Task FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2,
            TArg3 arg3, object state, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod,
            TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task StartNew(Action action)
        {
            return _taskFactory.StartNew(action);
        }

        public virtual Task<TResult> StartNew<TResult>(Func<TResult> function)
        {
            throw new NotImplementedException();
        }

        public virtual Task StartNew(Action<object> action, object state)
        {
            throw new NotImplementedException();
        }

        public virtual Task StartNew(Action action, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task StartNew(Action action, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> StartNew<TResult>(Func<TResult> function, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> StartNew<TResult>(Func<TResult> function, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task StartNew(Action<object> action, object state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task StartNew(Action<object> action, object state, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, TaskCreationOptions creationOptions)
        {
            throw new NotImplementedException();
        }

        public virtual Task StartNew(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> StartNew<TResult>(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task StartNew(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
        {
            throw new NotImplementedException();
        }
    }
}