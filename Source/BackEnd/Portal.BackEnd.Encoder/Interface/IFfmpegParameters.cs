using System.Diagnostics;

namespace FfmpegBackend.Interface
{
    public interface IFfmpegParameters
    {
        ProcessStartInfo GetStartInfo();
    }
}