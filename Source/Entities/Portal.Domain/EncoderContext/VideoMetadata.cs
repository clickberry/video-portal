// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.EncoderContext
{
    public sealed class VideoMetadata : IVideoMetadata
    {
        public string VideoFileHash { get; set; }
        public DateTime Created { get; set; }
        public string GeneralFormat { get; set; }
        public string GeneralFormatProfile { get; set; }
        public string GeneralCodecID { get; set; }
        public long GeneralFileSize { get; set; }
        public long GeneralDuration { get; set; }
        public int GeneralOverallBitRate { get; set; }
        public DateTime GeneralEncodedDate { get; set; }
        public DateTime GeneralTaggedDate { get; set; }
        public int VideoID { get; set; }
        public string VideoFormat { get; set; }
        public string VideoFormatInfo { get; set; }
        public string VideoFormatProfile { get; set; }
        public string VideoFormatSettingsCABAC { get; set; }
        public int VideoFormatSettingsRefFrames { get; set; }
        public string VideoFormatSettingsGOP { get; set; }
        public string VideoCodecID { get; set; }
        public string VideoCodecIDInfo { get; set; }
        public long VideoDuration { get; set; }
        public string VideoBitRateMode { get; set; }
        public int VideoBitRate { get; set; }
        public int VideoBitRateMinimum { get; set; }
        public int VideoBitRateMaximum { get; set; }
        public int VideoBitRateNominal { get; set; }
        public int VideoWidth { get; set; }
        public int VideoHeight { get; set; }
        public string VideoDisplayAspectRatio { get; set; }
        public string VideoFrameRateMode { get; set; }
        public double VideoFrameRate { get; set; }
        public double VideoFrameRateOriginal { get; set; }
        public double VideoFrameRateMinimum { get; set; }
        public double VideoFrameRateMaximum { get; set; }
        public string VideoColorSpace { get; set; }
        public string VideoChromaSubsampling { get; set; }
        public int VideoBitDepth { get; set; }
        public string VideoScanType { get; set; }
        public double VideoBitsPixelByFrame { get; set; }
        public long VideoStreamSize { get; set; }
        public string VideoTitle { get; set; }
        public string VideoLanguage { get; set; }
        public DateTime VideoEncodedDate { get; set; }
        public DateTime VideoTaggedDate { get; set; }
        public int VideoStreamsCount { get; set; }
        public double VideoRotation { get; set; }

        public int AudioID { get; set; }
        public string AudioFormat { get; set; }
        public string AudioFormatInfo { get; set; }
        public string AudioFormatProfile { get; set; }
        public string AudioCodecID { get; set; }
        public long AudioDuration { get; set; }
        public string AudioBitRateMode { get; set; }
        public int AudioBitRate { get; set; }
        public int AudioBitRateMinimum { get; set; }
        public int AudioBitRateMaximum { get; set; }
        public int AudioBitRateNominal { get; set; }
        public int AudioChannels { get; set; }
        public string AudioChannelPositions { get; set; }
        public int AudioSamplingRate { get; set; }
        public string AudioCompressionMode { get; set; }
        public long AudioStreamSize { get; set; }
        public string AudioTitle { get; set; }
        public string AudioLanguage { get; set; }
        public DateTime AudioEncodedDate { get; set; }
        public DateTime AudioTaggedDate { get; set; }
        public int AudioStreamsCount { get; set; }
    }
}