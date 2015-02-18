// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Portal.BackEnd.Encoder.Interface;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BackEnd.Encoder.StringBuilder
{
    public class ScreenshotEncodeStringBuilder : EncodeStringBuilderBase, IEncodeStringBuilder
    {
        private readonly ScreenshotEncodeData _encodeData;
        private readonly IScreenshotEncodeStringFactory _screenshotEncodeStringFactory;
        private readonly ITempFileManager _tempFileManager;

        public ScreenshotEncodeStringBuilder(ScreenshotEncodeData encodeData, IScreenshotEncodeStringFactory screenshotEncodeStringFactory, ITempFileManager tempFileManager)
            : base(encodeData)
        {
            _encodeData = encodeData;
            _screenshotEncodeStringFactory = screenshotEncodeStringFactory;
            _tempFileManager = tempFileManager;
        }

        public string GetFfmpegArguments()
        {
            const string template = @"-i ""{0}"" -f image2 {1} -frames:v 1 {2} -y ""{3}""";
            string imageOption = _screenshotEncodeStringFactory.GetImageOption(_encodeData.ScreenshotParam.TimeOffset);
            string videoFilter = _screenshotEncodeStringFactory.GetVideoFilter((int)_encodeData.ScreenshotParam.VideoRotation);
            string originSource = _tempFileManager.GetOriginalTempFilePath();
            string destinationSource = _tempFileManager.GetEncodingTempFilePath();

            string ffmpegString = String.Format(template, originSource, imageOption, videoFilter, destinationSource);

            Trace.TraceInformation("Screenshot encoding params for file {0}: {1}", _encodeData.SourceFileId, ffmpegString);

            return ffmpegString;
        }
    }
}