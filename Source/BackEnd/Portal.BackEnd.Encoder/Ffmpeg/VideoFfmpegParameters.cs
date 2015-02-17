using System.Diagnostics;
using FfmpegBackend.Data;
using FfmpegBackend.Interface;

namespace FfmpegBackend.Ffmpeg
{
    public class VideoFfmpegParameters : IFfmpegParameters
    {
        public VideoFfmpegParameters(VideoEncodeData encodeData)
        {
           
        }

        public ProcessStartInfo GetStartInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}