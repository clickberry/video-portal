// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Asp.Infrastructure.Extensions
{
    public static class TimeoutTaskExtensions
    {
        public static Task<T> Timeout<T>(this Task<T> task, TimeSpan timeout)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();

            Task delay = Task.Delay(timeout);

            Task.WhenAny(task, delay)
                .ContinueWith(
                    p =>
                    {
                        if (task.IsCompleted)
                        {
                            if (task.IsCanceled)
                            {
                                taskCompletionSource.SetCanceled();
                            }
                            else if (task.IsFaulted)
                            {
                                taskCompletionSource.SetException(task.Exception ?? new AggregateException("Task has failed with no exception!"));
                            }
                            else
                            {
                                taskCompletionSource.SetResult(task.Result);
                            }
                        }
                        else if (delay.IsCompleted)
                        {
                            if (delay.IsCanceled)
                            {
                                taskCompletionSource.SetException(new ApplicationException("Timeout task has been canceled!"));
                            }
                            else if (delay.IsFaulted)
                            {
                                taskCompletionSource.SetException(new AggregateException("Timeout task has failed!", delay.Exception ?? new AggregateException("Task has failed with no exception!")));
                            }
                            else
                            {
                                taskCompletionSource.SetException(new TimeoutException("Task has timed out!"));
                            }
                        }
                        else
                        {
                            taskCompletionSource.SetException(new ApplicationException("Unexpected timeout task behavior has occured!"));
                        }
                    });

            return taskCompletionSource.Task;
        }
    }
}