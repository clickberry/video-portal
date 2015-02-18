// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Portal.DAL.Multimedia;
using Portal.Domain.EncoderContext;

namespace Portal.DAL.Infrastructure.Multimedia
{
    public sealed class VideoMetadataProvider : IVideoMetadataProvider
    {
        private readonly IVideoMetadataParser _videoMetadataParser;

        public VideoMetadataProvider(IVideoMetadataParser videoMetadataParser)
        {
            if (videoMetadataParser == null)
            {
                throw new ArgumentNullException("videoMetadataParser");
            }

            _videoMetadataParser = videoMetadataParser;
        }

        public async Task<IVideoMetadata> GetMetadata(string fileUri)
        {
            return await _videoMetadataParser.GetVideoMetadata(fileUri);
        }
    }
}