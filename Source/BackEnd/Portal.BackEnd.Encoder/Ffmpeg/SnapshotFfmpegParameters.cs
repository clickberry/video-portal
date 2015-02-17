using System.Diagnostics;
using FfmpegBackend.Data;
using FfmpegBackend.Interface;

namespace FfmpegBackend.Ffmpeg
{
    public class SnapshotFfmpegParameters : IFfmpegParameters
    {
        public SnapshotFfmpegParameters(SnapshotEncodeData encodeData)
        {
            
        }

        public ProcessStartInfo GetStartInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}