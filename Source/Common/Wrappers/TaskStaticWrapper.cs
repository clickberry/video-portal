// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wrappers
{
    public class TaskStaticWrapper
    {
        public virtual int? CurrentId
        {
            get { return Task.CurrentId; }
        }

        public virtual TaskFactoryWrapper Factory
        {
            get
            {
                return new TaskFactoryWrapper
                {
                    TaskFactory = new TaskFactory()
                };
            }
        }

        public virtual Task Delay(int millisecondsDelay)
        {
            return Task.Delay(millisecondsDelay);
        }

        public static Task Delay(TimeSpan delay)
        {
            return Task.Delay(delay);
        }

        public static Task Delay(int millisecondsDelay, CancellationToken cancellationToken)
        {
            return Task.Delay(millisecondsDelay, cancellationToken);
        }

        public static Task Delay(TimeSpan delay, CancellationToken cancellationToken)
        {
            return Task.Delay(delay, cancellationToken);
        }
    }
}