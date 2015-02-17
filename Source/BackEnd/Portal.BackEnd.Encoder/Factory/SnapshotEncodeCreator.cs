using FfmpegBackend.Data;
using FfmpegBackend.EncodeTask;
using FfmpegBackend.Ffmpeg;
using FfmpegBackend.Interface;
using RestSharp;
using RestSharp.Deserializers;
using Wrappers;

namespace FfmpegBackend.Factory
{
    public class SnapshotEncodeCreator:IEncodeCreator
    {
        public IFfmpeg FfmpegCreate(IFfmpegParameters ffmpegParameters, ProcessWrapper process, IWatchDogTimer watchDogTimer)
        {
            return new SnapshotFfmpeg(ffmpegParameters);
        }

        public IFfmpegParameters FfmpegParametersCreate(EncodeDataBase encodeData)
        {
            return new SnapshotFfmpegParameters((SnapshotEncodeData)encodeData);
        }

        public IEncodeTask EncodeTaskCreate()
        {
            return new SnapshotEncodeTask();
        }
    }
}