// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.StringBuilder;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BackEnd.Encoder.Factory
{
    public class ScreenshotEncodeCreator : EncodeCreatorBase, IEncodeCreator
    {
        private readonly ScreenshotEncodeData _data;

        public ScreenshotEncodeCreator(ScreenshotEncodeData data)
        {
            _data = data;
        }

        public IFfmpegParser CreateFfmpegParser()
        {
            return new ScreenshotFfmpegParser();
        }

        public IEncodeStringFactory CreateEncodeStringFactory()
        {
            return new ScreenshotEncodeStringFactory();
        }

        public IEncodeStringBuilder CreateEncodeStringBuilder(ITempFileManager tempFileManager, IEncodeStringFactory encodeStringFactory)
        {
            return new ScreenshotEncodeStringBuilder(_data, (IScreenshotEncodeStringFactory)encodeStringFactory, tempFileManager);
        }
    }
}