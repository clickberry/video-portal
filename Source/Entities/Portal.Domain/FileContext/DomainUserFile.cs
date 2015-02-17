// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.FileContext
{
    public sealed class DomainUserFile
    {
        public string FileId { get; set; }

        public string UserId { get; set; }

        public string FileName { get; set; }

        public long FileLength { get; set; }

        public string ContentType { get; set; }

        public string FileUri { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}