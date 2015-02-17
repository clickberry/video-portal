// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.Domain.EncoderContext;

namespace Portal.DAL.Multimedia
{
    public interface IVideoMetadataParser
    {
        Task<IVideoMetadata> GetVideoMetadata(string fileName);
    }
}