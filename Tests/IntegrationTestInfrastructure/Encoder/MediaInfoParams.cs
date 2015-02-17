namespace IntegrationTestInfrastructure.Encoder
{
    public class MediaInfoParams
    {
        public const string FileSize = "General;%FileSize%";
        public const string Duration = "General;%Duration%";
        public const string Container = "General;%Format%";

        public const string VideoFps = "Video;%FrameRate%";
        public const string VideoBps = "Video;%BitRate%";
        public const string VideoKeyFrame = "Video;%Format_Settings_GOP%";
        public const string VideoCodec = "Video;%Format%";
        public const string VideoFormatProfile = "Video;%Format_Profile%";
        public const string VideoWidth = "Video;%Width%";
        public const string VideHeight = "Video;%Height%";

        public const string AudioCodec = "Audio;%Format%";
        public const string AudioBps = "Audio;%BitRate%";
        public const string AudioChannel = "Audio;%Channel(s)%";
        public const string AudioProfile = "Audio;%Format_Profile%";

        public const string Option = "Inform";
    }
}