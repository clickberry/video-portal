// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.BackEnd.Encoder.Ffmpeg
{
    public class FfmpegConstant
    {
        //Containers
        public const string Mp4FfmpegContainer = "mp4";
        public const string WebmFfmpegContainer = "webm";

        //Video codecs
        public const string AvcCodecLib = "libx264";
        public const string Vp8CodecLib = "libvpx";

        //Audio codecs
        public const string AacCodecLib = "aac -strict experimental"; //"libvo_aacenc";
        public const string VorbisCodecLib = "libvorbis";
    }
}