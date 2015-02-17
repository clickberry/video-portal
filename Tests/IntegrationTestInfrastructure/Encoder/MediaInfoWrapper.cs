using MediaInfoLibrary;

namespace IntegrationTestInfrastructure.Encoder
{
    public class MediaInfoWrapper:IMediaInfo
    {
        private readonly MediaInfo _mediaInfo;

        public MediaInfoWrapper()
        {
            _mediaInfo = new MediaInfo();
        }

        public int Open(string path)
        {
            return _mediaInfo.Open(path);
        }

        public string Option(string option, string value)
        {
            _mediaInfo.Option(option, value);
            return _mediaInfo.Inform();
        }
        
        public void Close()
        {
            _mediaInfo.Close();
        }
    }
}