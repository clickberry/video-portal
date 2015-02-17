// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Portal.BackEnd.Encoder.Interface;
using Portal.Domain.BackendContext.Entity;

namespace Portal.BackEnd.Encoder.StringBuilder
{
    public class VideoEncodeStringBuilder : EncodeStringBuilderBase, IEncodeStringBuilder
    {
        private readonly VideoEncodeData _encodeData;
        private readonly ITempFileManager _tempFileManager;
        private readonly IVideoEncodeStringFactory _videoEncodeStringFactory;

        public VideoEncodeStringBuilder(VideoEncodeData encodeData, IVideoEncodeStringFactory videoEncodeStringFactory, ITempFileManager tempFileManager)
            : base(encodeData)
        {
            _encodeData = encodeData;
            _videoEncodeStringFactory = videoEncodeStringFactory;
            _tempFileManager = tempFileManager;
        }

        public string GetFfmpegArguments()
        {
            const string template = @"-i ""{0}"" {1} {2} {3} -y ""{4}""";
            string videoCodecLib = _videoEncodeStringFactory.GetVideoCodecLib(_encodeData.VideoParam.VideoCodec);
            string videoCodecOption = _videoEncodeStringFactory.GetVideoCodecOption(_encodeData.VideoParam.VideoCodec, _encodeData.VideoParam.VideoProfile);
            string videoFilter = _videoEncodeStringFactory.GetVideoFilter((int)_encodeData.VideoParam.VideoRotation);
            string audioCodecLib = _videoEncodeStringFactory.GetAudioCodecLib(_encodeData.AudioParam.AudioCodec);

            string videoString = _videoEncodeStringFactory.GetVideoString(videoCodecLib, videoCodecOption, videoFilter,
                _encodeData.VideoParam.VideoBitrate,
                _encodeData.VideoParam.FrameRate,
                _encodeData.VideoParam.KeyFrameRate,
                _encodeData.VideoParam.VideoWidth,
                _encodeData.VideoParam.VideoHeight,
                _encodeData.IsVideoCopy);

            string containerString = _videoEncodeStringFactory.GetContainerString(_encodeData.VideoParam.MediaContainer);
            string audioString = _videoEncodeStringFactory.GetAudioString(audioCodecLib,
                _encodeData.AudioParam.AudioBitrate,
                _encodeData.IsAudioCopy);
            string originSource = _tempFileManager.GetOriginalTempFilePath();
            string destinationSource = _tempFileManager.GetEncodingTempFilePath();

            string ffmpegString = String.Format(template, originSource, containerString, videoString, audioString, destinationSource);

            Trace.TraceInformation("Video encoding params for file {0}: {1}", _encodeData.SourceFileId, ffmpegString);

            return ffmpegString;
        }
    }
}