using Backend.Domain;
using FfmpegBackend.Data;
using FfmpegBackend.Interface;

namespace FfmpegBackend.Ffmpeg
{
    public class SnapshotFfmpeg : IFfmpeg
    {
        public SnapshotFfmpeg(IFfmpegParameters ffmpegData)
        {
            
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public EncoderStatus GetStatus()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}