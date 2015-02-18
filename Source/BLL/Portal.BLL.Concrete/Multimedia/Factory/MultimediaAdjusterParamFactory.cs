// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Text.RegularExpressions;
using Portal.BLL.Concrete.Multimedia.AdjusterParam;
using Portal.Domain.EncoderContext;
using Portal.Exceptions.Multimedia;

namespace Portal.BLL.Concrete.Multimedia.Factory
{
    public class MultimediaAdjusterParamFactory : IMultimediaAdjusterParamFactory
    {
        public VideoAdjusterParam CreateVideoParam(IVideoMetadata videoMetadata)
        {
            CheckVideoStream(videoMetadata.VideoStreamsCount);
            int keyFrameRate = ParseKeyFrameRate(videoMetadata.VideoFormatSettingsGOP);
            VideoAdjusterParam videoParam = CreateVideoParam(videoMetadata, keyFrameRate);

            return videoParam;
        }

        public AudioAdjusterParam CreateAudioParam(IVideoMetadata videoMetadata)
        {
            CheckAudioStream(videoMetadata.AudioStreamsCount);

            return new AudioAdjusterParam
            {
                AudioCodec = videoMetadata.AudioFormat,
                AudioBitrate = videoMetadata.AudioBitRate,
                AudioChannels = videoMetadata.AudioChannels,
                IsExistAudioStream = videoMetadata.AudioStreamsCount != 0,
                AudioSampleRate = videoMetadata.AudioSamplingRate
            };
        }

        private int ParseKeyFrameRate(string keyFrameRate)
        {
            int keyFrame = 0;
            if (String.IsNullOrEmpty(keyFrameRate))
            {
                return 0;
            }
            var regex = new Regex("N=([0-9]+)", RegexOptions.Compiled | RegexOptions.Singleline);
            Match match = regex.Match(keyFrameRate);
            if (match.Groups.Count == 2)
            {
                int.TryParse(match.Groups[1].ToString(), out keyFrame);
            }
            return keyFrame;
        }

        private VideoAdjusterParam CreateVideoParam(IVideoMetadata videoMetadata, int keyFrameRate)
        {
            return new VideoAdjusterParam
            {
                MediaContainer = videoMetadata.GeneralFormat,
                VideoCodec = videoMetadata.VideoFormat,
                VideoProfile = videoMetadata.VideoFormatProfile,
                VideoBitrate = videoMetadata.VideoBitRate,
                FrameRate = videoMetadata.VideoFrameRate,
                FrameRateMode = videoMetadata.VideoFrameRateMode,
                KeyFrameRate = keyFrameRate,
                VideoRotation = videoMetadata.VideoRotation
            };
        }

        private void CheckVideoStream(int videoStreamCount)
        {
            if (videoStreamCount != 1)
            {
                throw new AggregateException(new VideoFormatException(ParamType.VideoStreamCount));
            }
        }

        private void CheckAudioStream(int audioStremCount)
        {
            if (audioStremCount > 1)
            {
                throw new AggregateException(new VideoFormatException(ParamType.AudioStreamCount));
            }
        }
    }
}