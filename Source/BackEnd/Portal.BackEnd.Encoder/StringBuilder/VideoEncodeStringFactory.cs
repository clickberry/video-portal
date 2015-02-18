// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.Interface;
using Portal.Domain.BackendContext.Constant;

namespace Portal.BackEnd.Encoder.StringBuilder
{
    public class VideoEncodeStringFactory : EncodeStringFactoryBase, IVideoEncodeStringFactory
    {
        public string GetVideoCodecLib(string videoCodec)
        {
            switch (videoCodec)
            {
                case MetadataConstant.AvcCodec:
                    return FfmpegConstant.AvcCodecLib;
                case MetadataConstant.Vp8Codec:
                    return FfmpegConstant.Vp8CodecLib;

                default:
                    return null;
            }
        }

        public string GetVideoCodecOption(string videoCodec, string videoProfile)
        {
            switch (videoCodec)
            {
                case MetadataConstant.AvcCodec:
                    return String.Format("-profile:v {0}", videoProfile);
                case MetadataConstant.Vp8Codec:
                    return String.Format("-quality good -cpu-used 5 -threads {0}", Environment.ProcessorCount - 1);

                default:
                    return null;
            }
        }

        public string GetAudioCodecLib(string audioCodec)
        {
            switch (audioCodec)
            {
                case MetadataConstant.AacCodec:
                    return FfmpegConstant.AacCodecLib;
                case MetadataConstant.VorbisCodec:
                    return FfmpegConstant.VorbisCodecLib;

                default:
                    return null;
            }
        }

        public string GetContainerString(string container)
        {
            switch (container)
            {
                case MetadataConstant.Mp4Container:
                    return String.Format("-f {0}", FfmpegConstant.Mp4FfmpegContainer);
                case MetadataConstant.WebmContainer:
                    return String.Format("-f {0}", FfmpegConstant.WebmFfmpegContainer);

                default:
                    return null;
            }
        }

        public string GetAudioString(string audioCodecLib, int audioBitrate, bool isAudioCopy)
        {
            string audioStr = "-acodec {0} -b:a {1}";

            if (isAudioCopy)
            {
                audioStr = "-acodec copy";
            }
            else
            {
                audioStr = String.IsNullOrEmpty(audioCodecLib) ? "" : String.Format(audioStr, audioCodecLib, audioBitrate);
            }
            return audioStr;
        }

        public string GetVideoString(string videoCodecLib, string videoCodecOption, string videoFilter, int videoBitrate, double frameRate, int keyFrameRate, int width, int height, bool isVideoCopy)
        {
            string videoStr = "-vcodec {0} -b:v {1} -r {2} -g {3} -s {4}x{5} {6} {7}";

            if (isVideoCopy)
            {
                videoStr = "-vcodec copy";
            }
            else
            {
                videoStr = String.Format(CultureInfo.InvariantCulture,
                    videoStr, videoCodecLib, videoBitrate, frameRate, keyFrameRate, width,
                    height, videoCodecOption, videoFilter);
            }
            return videoStr;
        }
    }
}