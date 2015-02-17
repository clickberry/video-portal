// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace MiddleEnd.Worker.Abstract
{
    public interface ITaskStateManager
    {
        /// <summary>
        ///     Start updater.
        /// </summary>
        void Start();

        /// <summary>
        ///     Stop updater.
        /// </summary>
        void Stop();
    }
}