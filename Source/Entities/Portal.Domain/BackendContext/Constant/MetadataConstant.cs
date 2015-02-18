// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.Domain.BackendContext.Constant
{
    public class MetadataConstant
    {
        public const string JpegFormat = "jpg";

        //Containers
        public const string Mp4Container = "MPEG-4";
        public const string WebmContainer = "WebM";

        //Video codecs
        public const string AvcCodec = "AVC";
        public const string Vp8Codec = "VP8";

        //Video profile
        public const string AvcMainProfile = "Main";
        public const string AvcBaselineProfile = "Baseline";

        //Audio codecs
        public const string AacCodec = "AAC";
        public const string VorbisCodec = "Vorbis";

        //Frame rate
        public const string VariableFrameRate = "VFR";
        public const string ConstantFrameRate = "Constant";
    }
}