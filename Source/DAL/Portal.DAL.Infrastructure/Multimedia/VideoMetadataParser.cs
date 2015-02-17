// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;
using MediaInfoLibrary;
using Portal.DAL.Multimedia;
using Portal.Domain.EncoderContext;

namespace Portal.DAL.Infrastructure.Multimedia
{
    public sealed class VideoMetadataParser : IVideoMetadataParser
    {
        private readonly Lazy<Task<IMediaInfo>> _mediaInfoPromiseLazy;

        private readonly DateTime _minimumDateTime;

        public VideoMetadataParser(Lazy<Task<IMediaInfo>> mediaInfoPromise)
        {
            _mediaInfoPromiseLazy = mediaInfoPromise;
            _minimumDateTime = new DateTime(1970, 1, 1);
        }

        private Task<IMediaInfo> MediaInfoPromise
        {
            get { return _mediaInfoPromiseLazy.Value; }
        }

        public async Task<IVideoMetadata> GetVideoMetadata(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            IMediaInfo mediaInfo = await MediaInfoPromise;

            if (await mediaInfo.Open(fileName) == 0)
            {
                throw new IOException(string.Format("Failed to open media file: {0}", fileName));
            }

            VideoMetadata metadata = null;
            Exception exception = null;

            try
            {
                metadata = new VideoMetadata
                {
                    GeneralFormat = await mediaInfo.GetStringValue(StreamKind.General, "Format"),
                    GeneralFormatProfile = await mediaInfo.GetStringValue(StreamKind.General, "Format_Profile"),
                    GeneralCodecID = await mediaInfo.GetStringValue(StreamKind.General, "CodecID"),
                    GeneralFileSize = await mediaInfo.GetLongValue(StreamKind.General, "FileSize"),
                    GeneralDuration = await mediaInfo.GetDurationValue(StreamKind.General, "Duration/String3"),
                    GeneralOverallBitRate = await mediaInfo.GetIntegerValue(StreamKind.General, "BitRate"),
                    GeneralEncodedDate = FixDateTime(await mediaInfo.GetDateValue(StreamKind.General, "Encoded_Date")),
                    GeneralTaggedDate = FixDateTime(await mediaInfo.GetDateValue(StreamKind.General, "Tagged_Date")),
                    VideoStreamsCount = await mediaInfo.GetIntegerValue(StreamKind.Video, "StreamCount"),
                    VideoFormat = await mediaInfo.GetStringValue(StreamKind.Video, "Format"),
                    VideoFormatInfo = await mediaInfo.GetStringValue(StreamKind.Video, "Format/Info"),
                    VideoFormatProfile = await mediaInfo.GetStringValue(StreamKind.Video, "Format_Profile"),
                    VideoFormatSettingsCABAC = await mediaInfo.GetStringValue(StreamKind.Video, "Format_Settings_CABAC"),
                    VideoFormatSettingsRefFrames = await mediaInfo.GetIntegerValue(StreamKind.Video, "Format_Settings_RefFrames"),
                    VideoFormatSettingsGOP = await mediaInfo.GetStringValue(StreamKind.Video, "Format_Settings_GOP"),
                    VideoCodecID = await mediaInfo.GetStringValue(StreamKind.Video, "CodecID"),
                    VideoCodecIDInfo = await mediaInfo.GetStringValue(StreamKind.Video, "CodecID/Info"),
                    VideoDuration = await mediaInfo.GetDurationValue(StreamKind.Video, "Duration/String3"),
                    VideoBitRateMode = await mediaInfo.GetStringValue(StreamKind.Video, "BitRate_Mode"),
                    VideoBitRate = await mediaInfo.GetIntegerValue(StreamKind.Video, "BitRate"),
                    VideoBitRateMinimum = await mediaInfo.GetIntegerValue(StreamKind.Video, "BitRate_Minimum"),
                    VideoBitRateMaximum = await mediaInfo.GetIntegerValue(StreamKind.Video, "BitRate_Maximum"),
                    VideoBitRateNominal = await mediaInfo.GetIntegerValue(StreamKind.Video, "BitRate_Nominal"),
                    VideoWidth = await mediaInfo.GetIntegerValue(StreamKind.Video, "Width"),
                    VideoHeight = await mediaInfo.GetIntegerValue(StreamKind.Video, "Height"),
                    VideoDisplayAspectRatio = await mediaInfo.GetStringValue(StreamKind.Video, "DisplayAspectRatio/String"),
                    VideoFrameRateMode = await mediaInfo.GetStringValue(StreamKind.Video, "FrameRate_Mode"),
                    VideoFrameRate = await mediaInfo.GetDoubleValue(StreamKind.Video, "FrameRate"),
                    VideoFrameRateMinimum = await mediaInfo.GetDoubleValue(StreamKind.Video, "FrameRate_Minimum"),
                    VideoFrameRateMaximum = await mediaInfo.GetDoubleValue(StreamKind.Video, "FrameRate_Maximum"),
                    VideoFrameRateOriginal = await mediaInfo.GetDoubleValue(StreamKind.Video, "FrameRate_Original"),
                    VideoColorSpace = await mediaInfo.GetStringValue(StreamKind.Video, "ColorSpace"),
                    VideoChromaSubsampling = await mediaInfo.GetStringValue(StreamKind.Video, "Colorimetry"),
                    VideoBitDepth = await mediaInfo.GetIntegerValue(StreamKind.Video, "Resolution"),
                    VideoScanType = await mediaInfo.GetStringValue(StreamKind.Video, "ScanType"),
                    VideoBitsPixelByFrame = await mediaInfo.GetDoubleValue(StreamKind.Video, "Bits-(Pixel*Frame)"),
                    VideoStreamSize = await mediaInfo.GetLongValue(StreamKind.Video, "StreamSize"),
                    VideoTitle = await mediaInfo.GetStringValue(StreamKind.Video, "Title"),
                    VideoLanguage = await mediaInfo.GetStringValue(StreamKind.Video, "Language"),
                    VideoEncodedDate = FixDateTime(await mediaInfo.GetDateValue(StreamKind.Video, "Encoded_Date")),
                    VideoTaggedDate = FixDateTime(await mediaInfo.GetDateValue(StreamKind.Video, "Tagged_Date")),
                    VideoRotation = await mediaInfo.GetDoubleValue(StreamKind.Video, "Rotation"),
                    AudioStreamsCount = await mediaInfo.GetIntegerValue(StreamKind.Audio, "StreamCount"),
                    AudioFormat = await mediaInfo.GetStringValue(StreamKind.Audio, "Format"),
                    AudioFormatInfo = await mediaInfo.GetStringValue(StreamKind.Audio, "Format/Info"),
                    AudioFormatProfile = await mediaInfo.GetStringValue(StreamKind.Audio, "Format_Profile"),
                    AudioCodecID = await mediaInfo.GetStringValue(StreamKind.Audio, "CodecID"),
                    AudioDuration = await mediaInfo.GetDurationValue(StreamKind.Audio, "Duration/String3"),
                    AudioBitRateMode = await mediaInfo.GetStringValue(StreamKind.Audio, "BitRate_Mode"),
                    AudioBitRate = await mediaInfo.GetIntegerValue(StreamKind.Audio, "BitRate"),
                    AudioBitRateMinimum = await mediaInfo.GetIntegerValue(StreamKind.Audio, "BitRate_Minimum"),
                    AudioBitRateMaximum = await mediaInfo.GetIntegerValue(StreamKind.Audio, "BitRate_Maximum"),
                    AudioBitRateNominal = await mediaInfo.GetIntegerValue(StreamKind.Audio, "BitRate_Nominal"),
                    AudioChannels = await mediaInfo.GetIntegerValue(StreamKind.Audio, "Channel(s)"),
                    AudioChannelPositions = await mediaInfo.GetStringValue(StreamKind.Audio, "ChannelPositions"),
                    AudioSamplingRate = await mediaInfo.GetIntegerValue(StreamKind.Audio, "SamplingRate"),
                    AudioCompressionMode = await mediaInfo.GetStringValue(StreamKind.Audio, "Compression_Mode"),
                    AudioStreamSize = await mediaInfo.GetLongValue(StreamKind.Audio, "StreamSize"),
                    AudioTitle = await mediaInfo.GetStringValue(StreamKind.Audio, "Title"),
                    AudioLanguage = await mediaInfo.GetStringValue(StreamKind.Audio, "Language"),
                    AudioEncodedDate = FixDateTime(await mediaInfo.GetDateValue(StreamKind.Audio, "Encoded_Date")),
                    AudioTaggedDate = FixDateTime(await mediaInfo.GetDateValue(StreamKind.Audio, "Tagged_Date"))
                };
            }
            catch (Exception e)
            {
                exception = e;
            }

            await mediaInfo.Close();

            if (exception != null)
            {
                throw new ApplicationException(string.Format("Failed to receive metadata for file {0}", fileName), exception);
            }

            return metadata;
        }

        private DateTime FixDateTime(DateTime dateTime)
        {
            return dateTime < _minimumDateTime ? _minimumDateTime : dateTime;
        }
    }
}