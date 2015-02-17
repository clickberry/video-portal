// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.BillingContext
{
    public class DomainEvent
    {
        /// <summary>
        ///     Gets or sets an event identifier.
        /// </summary>
        public string Id { get; set; }

        public DateTime Created { get; set; }

        public bool LiveMode { get; set; }

        public EventType Type { get; set; }

        /// <summary>
        ///     Gets or sets an object identifier in the event.
        /// </summary>
        public string ObjectId { get; set; }

        public dynamic Object { get; set; }
    }
}