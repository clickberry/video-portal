// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.BackendContext.Entity.Base
{
    public interface IEncodeData
    {
        string SourceFileId { get; set; }

        string ContentType { get; set; }
    }
}