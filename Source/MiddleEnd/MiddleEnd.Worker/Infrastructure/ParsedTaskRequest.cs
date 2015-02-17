// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.Domain.ProcessedVideoContext;

namespace MiddleEnd.Worker.Infrastructure
{
    /// <summary>
    ///     Worker request entity.
    /// </summary>
    public class ParsedTaskRequest
    {
        /// <summary>
        ///     Gets or sets an accept task types.
        /// </summary>
        public List<ProcessedEntityType> Accepts { get; set; }

        /// <summary>
        ///     Gets or sets an user agent.
        /// </summary>
        public string UserAgent { get; set; }
    }
}