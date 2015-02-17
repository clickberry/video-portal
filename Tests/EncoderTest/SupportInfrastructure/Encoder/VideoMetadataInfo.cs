using System.Text.RegularExpressions;
using Portal.Domain;
using PortalEncoder.Exceptions;

namespace IntegrationTestInfrastructure.Encoder
{
    public class VideoMetadataInfo
    {
        private readonly IMediaInfo _mediaInfo;

        public VideoMetadataInfo(IMediaInfo mediaInfo)
        {
            _mediaInfo = mediaInfo;
        }

        public VideoMediaInfo GetMetadata(string filePath)
        {
            var metadata = new VideoMediaInfo();

            Open(filePath);

            metadata.AudioBitRate = GetIntValue(MediaInfoParams.AudioBps);
            metadata.AudioChannels = GetIntValue(MediaInfoParams.AudioChannel);
            metadata.VideoDuration = GetIntValue(MediaInfoParams.Duration);
            metadata.VideoHeight = GetIntValue(MediaInfoParams.VideHeight);
            metadata.VideoWidth = GetIntValue(MediaInfoParams.VideoWidth);
            metadata.VideoBitRate = GetIntValue(MediaInfoParams.VideoBps);
            //

            metadata.GeneralFileSize = GetLongValue(MediaInfoParams.FileSize);
            //

            metadata.VideoFrameRate = GetDoubleValue(MediaInfoParams.VideoFps);
            //

            metadata.AudioFormat = _mediaInfo.Option(MediaInfoParams.Option, MediaInfoParams.AudioCodec);
            metadata.AudioFormatProfile = _mediaInfo.Option(MediaInfoParams.Option, MediaInfoParams.AudioProfile);
            metadata.GeneralFormat = _mediaInfo.Option(MediaInfoParams.Option, MediaInfoParams.Container);
            metadata.VideoFormat = _mediaInfo.Option(MediaInfoParams.Option, MediaInfoParams.VideoCodec);
            metadata.VideoFormatSettingsGOP = _mediaInfo.Option(MediaInfoParams.Option, MediaInfoParams.VideoKeyFrame);
            //

            SetVideoFormatProfile(metadata);
            //

            Close();

            return metadata;
        }

        private void Open(string filePath)
        {
            if (_mediaInfo.Open(filePath) <= 0)
            {
                throw new MediaFormatException("FilePath");
            }
        }

        private void Close()
        {
            _mediaInfo.Close();
        }

        private void SetVideoFormatProfile(VideoMediaInfo metadata)
        {
            string val = _mediaInfo.Option(MediaInfoParams.Option, MediaInfoParams.VideoFormatProfile);
            if (val == null)
            {
                return;
            }

            string[] splitStr = Regex.Split(val, "@");
            metadata.VideoFormatProfile = splitStr[0];
        }

        private int GetIntValue(string paramName)
        {
            int intVal;
            string strVal = _mediaInfo.Option(MediaInfoParams.Option, paramName);
            int.TryParse(strVal, out intVal);
            return intVal;
        }

        private long GetLongValue(string paramName)
        {
            long intVal;
            string strVal = _mediaInfo.Option(MediaInfoParams.Option, paramName);
            long.TryParse(strVal, out intVal);
            return intVal;
        }

        private double GetDoubleValue(string paramName)
        {
            double intVal;
            string strVal = _mediaInfo.Option(MediaInfoParams.Option, paramName);
            double.TryParse(strVal, out intVal);
            return intVal;
        }
    }
}