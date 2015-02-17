// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Portal.Domain.EncoderContext;
using Portal.Domain.ProcessedVideoContext;

namespace Portal.BLL.Concrete.Infrastructure.ProcessedEntity
{
    public interface IProcessedEntityGenerator<TEntity> where TEntity : IProcessedEntity
    {
        List<TEntity> Generate(IVideoMetadata videoMetadata);
    }
}