// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.ProjectContext
{
    public interface IProjectProcessedEntity
    {
        string UserId { get; set; }
        string ProjectId { get; set; }
        string FileUri { get; set; }
        string FileId { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        string ContentType { get; set; }
    }
}