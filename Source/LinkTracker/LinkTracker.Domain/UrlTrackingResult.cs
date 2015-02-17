// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace LinkTracker.Domain
{
    public class UrlTrackingResult
    {
        public string ProjectId { get; set; }

        public Uri Redirect { get; set; }

        public string SubscriptionId { get; set; }

        /// <summary>
        ///     Indicates whether url redirect should be accounted in the system.
        /// </summary>
        public bool IsAccountable { get; set; }
    }
}