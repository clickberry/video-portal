// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.Domain.ProjectContext
{
    public sealed class DomainProjectProcessedScreenshot : IProjectProcessedEntity
    {
        public string UserId { get; set; }

        public string ProjectId { get; set; }

        public string FileUri { get; set; }

        public string FileId { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string ContentType { get; set; }
    }
}