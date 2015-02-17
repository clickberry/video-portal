using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using Encoder;
using Encoder.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EncoderTest
{
    [TestClass]
    public class FfmpegTest
    {
        private const string SourcePath = @"C:\myVideos"; //@"\\aananyev_pc\video";
        private const string DestinationPath = @"C:\TestVideo";

        [TestMethod]
        public void Mp4EncodeTest()
        {
            Directory.CreateDirectory(DestinationPath);
            var filePathes = Directory.GetFiles(SourcePath);
            var videoCodec = new CodecData("AVC", "libx264", "Main", "Main", "Baseline");
            var audioCodec = new CodecData("MPEG Audio", "libmp3lame", "Layer 3", "Layer 3");
            var supportedAudioCodecs = new List<CodecData>()
                                           {
                                               audioCodec,
                                               new CodecData("AAC", null)
                                           };
            var serviceConfigurator = new MetadataServiceConfigurator("MPEG-4", "mp4", videoCodec, audioCodec, supportedAudioCodecs);

            StartEncode(serviceConfigurator, filePathes);
        }

        [TestMethod]
        public void WebmEncodeTest()
        {
            Directory.CreateDirectory(DestinationPath);
            var filePathes = Directory.GetFiles(SourcePath);
            var videoCodec = new CodecData("VP8", "libvpx");
            var audioCodec = new CodecData("Vorbis", "libvorbis");
            var supportedAudioCodecs = new List<CodecData>()
                                           {
                                               audioCodec
                                           };
            var serviceConfigurator = new MetadataServiceConfigurator("WebM", "webm", videoCodec, audioCodec, supportedAudioCodecs);

            StartEncode(serviceConfigurator, filePathes);
        }

        [TestMethod]
        public void CreateScreenshotTest()
        {
            Directory.CreateDirectory(DestinationPath);
            var filePathes = Directory.GetFiles(SourcePath);
            var videoCodec = new CodecData("VP8", "libvpx");
            var audioCodec = new CodecData("Vorbis", "libvorbis");
            var supportedAudioCodecs = new List<CodecData>()
                                           {
                                               audioCodec
                                           };
            var serviceConfigurator = new MetadataServiceConfigurator("WebM", "webm", videoCodec, audioCodec, supportedAudioCodecs);

            StartScreenshot(serviceConfigurator, filePathes);
        }

        private void StartEncode(MetadataServiceConfigurator serviceConfigurator, string[] filePathes)
        {
            var allSw = new Stopwatch();
            var decodeSw = new Stopwatch();
            foreach (var filePath in filePathes)
            {
                allSw.Start();
                Trace.WriteLine(String.Format("\n-------------Start decode file: {0}-------------\n", filePath));
                
                try
                {
                    var destinationFilePath = GetDestinationFilePath(filePath);
                    var mediaInfo = new MediaInfoWrapper();
                    var metadataInfo = new VideoMetadataInfo(mediaInfo);
                    var metadata = metadataInfo.GetMetadata(filePath);
                    var metadataService = new MetadataService(serviceConfigurator, metadata);
                    var stringBuilder = new FfmpegStringBuilder(metadataService, destinationFilePath);
                    var ffmpeg = new Ffmpeg(stringBuilder);
                    var ffmpegString = stringBuilder.GetStringForEncoder();

                    WriteFileInfo(ffmpegString, metadata);

                    decodeSw.Start();
                    var result = ffmpeg.StartEncodeProcess();
                    allSw.Stop();
                    decodeSw.Stop();
                    WriteFinishProcess(decodeSw.Elapsed, allSw.Elapsed, result);
                }
                catch (MediaFormatException ex)
                {
                    Trace.WriteLine(String.Format("Error File: {0}. Error Param: {1}", ex.VideoUri, ex.InvalidParameter));
                }
                finally
                {
                    Trace.WriteLine(String.Format("\n-------------Finish decode file: {0}-------------\n", filePath));
                }
            }
        }

        private void StartScreenshot(MetadataServiceConfigurator serviceConfigurator, string[] filePathes)
        {
            var allSw = new Stopwatch();
            var decodeSw = new Stopwatch();
            foreach (var filePath in filePathes)
            {
                allSw.Start();
                Trace.WriteLine(String.Format("\n-------------Start decode file: {0}-------------\n", filePath));

                try
                {
                    var destinationFilePath = GetDestinationFilePath(filePath);
                    var mediaInfo = new MediaInfoWrapper();
                    var metadataInfo = new VideoMetadataInfo(mediaInfo);
                    var metadata = metadataInfo.GetMetadata(filePath);
                    var metadataService = new MetadataService(serviceConfigurator, metadata);
                    var stringBuilder = new FfmpegStringBuilder(metadataService, destinationFilePath);
                    var ffmpeg = new Ffmpeg(stringBuilder);
                    var ffmpegString = stringBuilder.GetStringForScreenshot();

                    WriteFileInfo(ffmpegString, metadata);

                    decodeSw.Start();
                    var result = ffmpeg.StartScreenshotProcess();
                    allSw.Stop();
                    decodeSw.Stop();
                    WriteFinishProcess(decodeSw.Elapsed, allSw.Elapsed, result);
                }
                catch (MediaFormatException ex)
                {
                    Trace.WriteLine(String.Format("Error File: {0}. Error Param: {1}", ex.VideoUri, ex.InvalidParameter));
                }
                finally
                {
                    Trace.WriteLine(String.Format("\n-------------Finish decode file: {0}-------------\n", filePath));
                }
            }
        }

        private void WriteFinishProcess(TimeSpan decodeTime, TimeSpan allTime, int result)
        {
            Trace.WriteLine(String.Format("!!!Finish Process!!!\nReturn Code:\t{0}", result));
            Trace.WriteLine(String.Format("All Time:\t{0}", allTime));
            Trace.WriteLine(String.Format("Decode Time:\t{0}", decodeTime));
        }

        private void WriteFileInfo(string ffmpegString, VideoMetadata metadata)
        {
            Trace.WriteLine(String.Format("Width:\t\t{0}", metadata.Width));
            Trace.WriteLine(String.Format("Width:\t\t{0}", metadata.Height));
            Trace.WriteLine(String.Format("File Size:\t\t{0}", metadata.FileSize));
            Trace.WriteLine(String.Format("Container:\t\t{0}", metadata.Container));
            Trace.WriteLine(String.Format("Video Codec:\t\t{0}", metadata.VideoCodec));
            Trace.WriteLine(String.Format("Video Profile:\t\t{0}", metadata.VideoProfile));
            Trace.WriteLine(String.Format("Video Bitrate:\t\t{0}", metadata.VideoBps));
            Trace.WriteLine(String.Format("Audio Codec:\t\t{0}", metadata.AudioCodec));
            Trace.WriteLine(String.Format("Audio Profile:\t\t{0}", metadata.AudioProfile));
            Trace.WriteLine(String.Format("Audio Channel(s):\t\t{0}", metadata.AudioChannel));
            Trace.WriteLine(String.Format("Audio Bitrate:\t\t{0}", metadata.AudioBps));
            Trace.WriteLine(String.Format("Ffmpeg String:\t\t{0}", ffmpegString));
        }

        private string GetDestinationFilePath(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var name = fileInfo.Name;

            return Path.Combine(DestinationPath, name);
        }
    }
}
