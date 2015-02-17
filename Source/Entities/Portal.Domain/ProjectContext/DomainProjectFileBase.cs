// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.ProjectContext
{
    /// <summary>
    ///     Domain project file base.
    /// </summary>
    public abstract class DomainProjectFileBase
    {
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public string FileId { get; set; }

        public string FileName { get; set; }

        public string FileUri { get; set; }

        public long FileLength { get; set; }

        public string ContentType { get; set; }
    }
}