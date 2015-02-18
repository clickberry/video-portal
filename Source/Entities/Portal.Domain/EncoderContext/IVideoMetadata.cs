// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;

namespace Portal.Domain.EncoderContext
{
    public interface IVideoMetadata
    {
        string VideoFileHash { get; set; }

        DateTime Created { get; set; }

        string GeneralFormat { get; set; }
        string GeneralFormatProfile { get; set; }
        string GeneralCodecID { get; set; }
        long GeneralFileSize { get; set; }
        long GeneralDuration { get; set; }
        int GeneralOverallBitRate { get; set; }
        DateTime GeneralEncodedDate { get; set; }
        DateTime GeneralTaggedDate { get; set; }

        int VideoID { get; set; }
        string VideoFormat { get; set; }
        string VideoFormatInfo { get; set; }
        string VideoFormatProfile { get; set; }
        string VideoFormatSettingsCABAC { get; set; }
        int VideoFormatSettingsRefFrames { get; set; }
        string VideoFormatSettingsGOP { get; set; }
        string VideoCodecID { get; set; }
        string VideoCodecIDInfo { get; set; }
        long VideoDuration { get; set; }
        string VideoBitRateMode { get; set; }
        int VideoBitRate { get; set; }
        int VideoBitRateMinimum { get; set; }
        int VideoBitRateMaximum { get; set; }
        int VideoBitRateNominal { get; set; }
        int VideoWidth { get; set; }
        int VideoHeight { get; set; }
        string VideoDisplayAspectRatio { get; set; }
        string VideoFrameRateMode { get; set; }
        double VideoFrameRate { get; set; }
        double VideoFrameRateOriginal { get; set; }
        double VideoFrameRateMinimum { get; set; }
        double VideoFrameRateMaximum { get; set; }
        string VideoColorSpace { get; set; }
        string VideoChromaSubsampling { get; set; }
        int VideoBitDepth { get; set; }
        string VideoScanType { get; set; }
        double VideoBitsPixelByFrame { get; set; }
        long VideoStreamSize { get; set; }
        string VideoTitle { get; set; }
        string VideoLanguage { get; set; }
        DateTime VideoEncodedDate { get; set; }
        DateTime VideoTaggedDate { get; set; }
        int VideoStreamsCount { get; set; }
        double VideoRotation { get; set; }

        int AudioID { get; set; }
        string AudioFormat { get; set; }
        string AudioFormatInfo { get; set; }
        string AudioFormatProfile { get; set; }
        string AudioCodecID { get; set; }
        long AudioDuration { get; set; }
        string AudioBitRateMode { get; set; }
        int AudioBitRate { get; set; }
        int AudioBitRateMinimum { get; set; }
        int AudioBitRateMaximum { get; set; }
        int AudioBitRateNominal { get; set; }
        int AudioChannels { get; set; }
        string AudioChannelPositions { get; set; }
        int AudioSamplingRate { get; set; }
        string AudioCompressionMode { get; set; }
        long AudioStreamSize { get; set; }
        string AudioTitle { get; set; }
        string AudioLanguage { get; set; }
        DateTime AudioEncodedDate { get; set; }
        DateTime AudioTaggedDate { get; set; }
        int AudioStreamsCount { get; set; }
    }
}