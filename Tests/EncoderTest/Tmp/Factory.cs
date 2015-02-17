using PortalEncoder;

namespace EncoderTest.Tmp
{
    public class Factory
    {
        public static MetadataServiceConfigurator CreateMp4MetadataServiceConfigurator()
        {
            const string container = "MPEG-4";
            const string ffmpegContainer = "mp4";

            var videoCodec = new CodecData("AVC", "libx264", "Main", "Baseline");

            var audioCodec = new CodecData("MPEG Audio", "libmp3lame", "Layer 3");
            
            return new MetadataServiceConfigurator(container, ffmpegContainer, videoCodec, audioCodec, new CodecData("AAC", null));
        }

        public static MetadataServiceConfigurator CreateWebMMetadataServiceConfigurator()
        {
            const string container = "WebM";
            const string ffmpegContainer = "webm";

            var videoCodec = new CodecData("VP8", "libvpx");

            var audioCodec = new CodecData("Vorbis", "libvorbis");
            
            return new MetadataServiceConfigurator(container, ffmpegContainer, videoCodec, audioCodec);
        }
    }
}