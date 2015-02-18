// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.StringBuilder;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BackEnd.Encoder.Factory
{
    public class VideoEncodeCreator : EncodeCreatorBase, IEncodeCreator
    {
        private readonly VideoEncodeData _data;

        public VideoEncodeCreator(VideoEncodeData data)
        {
            _data = data;
        }

        public IFfmpegParser CreateFfmpegParser()
        {
            return new VideoFfmpegParser();
        }

        public IEncodeStringBuilder CreateEncodeStringBuilder(ITempFileManager tempFileManager, IEncodeStringFactory encodeStringFactoryBase)
        {
            return new VideoEncodeStringBuilder(_data, (IVideoEncodeStringFactory)encodeStringFactoryBase, tempFileManager);
        }

        public IEncodeStringFactory CreateEncodeStringFactory()
        {
            return new VideoEncodeStringFactory();
        }
    }
}