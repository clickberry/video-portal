// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.Domain.EncoderContext;

namespace Portal.DAL.Multimedia
{
    public interface IVideoMetadataProvider
    {
        Task<IVideoMetadata> GetMetadata(string fileUri);
    }
}