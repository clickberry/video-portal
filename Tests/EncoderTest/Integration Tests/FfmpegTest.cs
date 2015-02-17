using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using IntegrationTestInfrastructure.Encoder;
using MSTestExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Domain;
using PortalEncoder;
using PortalEncoder.Exceptions;
using PortalEncoder.Ffmpeg;

namespace EncoderTest
{
    [TestClass]
    public class FfmpegTest
    {
        private string _destinationPath;
        private string _sourcePath;

        [TestInitialize]
        public void Initialize()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
            _sourcePath = ConfigurationManager.AppSettings.Get("SourcePath");
            _destinationPath = ConfigurationManager.AppSettings.Get("DestinationPath");
        }

        [TestMethod]
        public void Mp4EncodeTest()
        {
            Directory.CreateDirectory(_destinationPath);
            string[] filePathes = Directory.GetFiles(_sourcePath);
            var videoCodec = new CodecData(FfmpegConstant.AvcCodec, FfmpegConstant.AvcCodecLib, FfmpegConstant.AvcMainProfile, FfmpegConstant.AvcMainProfile, FfmpegConstant.AvcBaselineProfile);
            var audioCodec = new CodecData(FfmpegConstant.AacCodec, FfmpegConstant.AacCodecLib);

            var serviceConfigurator = new MetadataServiceConfigurator(FfmpegConstant.Mp4Container, FfmpegConstant.Mp4FfmpegContainer, videoCodec, audioCodec);

            StartEncode(serviceConfigurator, filePathes);
        }

        [TestMethod]
        public void WebmEncodeTest()
        {
            Directory.CreateDirectory(_destinationPath);
            string[] filePathes = Directory.GetFiles(_sourcePath);
            var videoCodec = new CodecData(FfmpegConstant.Vp8Codec, FfmpegConstant.Vp8CodecLib);
            var audioCodec = new CodecData(FfmpegConstant.VorbisCodec, FfmpegConstant.VorbisCodecLib);

            var serviceConfigurator = new MetadataServiceConfigurator(FfmpegConstant.WebmContainer, FfmpegConstant.WebmFfmpegContainer, videoCodec, audioCodec);

            StartEncode(serviceConfigurator, filePathes);
        }

        [TestMethod]
        public void TheoraEncodeTest()
        {
            Directory.CreateDirectory(_destinationPath);
            string[] filePathes = Directory.GetFiles(_sourcePath);
            var videoCodec = new CodecData(FfmpegConstant.TheoraCodec, FfmpegConstant.TheoraCodecLib);
            var audioCodec = new CodecData(FfmpegConstant.VorbisCodec, FfmpegConstant.VorbisCodecLib);

            var serviceConfigurator = new MetadataServiceConfigurator(FfmpegConstant.OggContainer, FfmpegConstant.OggFfmpegContainer, videoCodec, audioCodec);

            StartEncode(serviceConfigurator, filePathes);
        }

        [TestMethod]
        public void CreateScreenshotTest()
        {
            Directory.CreateDirectory(_destinationPath);
            string[] filePathes = Directory.GetFiles(_sourcePath);

            var serviceConfigurator = new MetadataServiceConfigurator(null, null, null, null, null);

            StartScreenshot(serviceConfigurator, filePathes);
        }

        [TestMethod]
        public void Mp4EncodeWithDifferentResolutionTest()
        {
            Directory.CreateDirectory(_destinationPath);
            string[] filePathes = Directory.GetFiles(_sourcePath);
            var videoCodec = new CodecData(FfmpegConstant.AvcCodec, FfmpegConstant.AvcCodecLib, FfmpegConstant.AvcMainProfile, FfmpegConstant.AvcMainProfile, FfmpegConstant.AvcBaselineProfile);
            var audioCodec = new CodecData(FfmpegConstant.AacCodec, FfmpegConstant.AacCodecLib);

            var resolutions = new[] {Int16.MaxValue, 500, 400, 100, 10};
            var serviceConfigurator = new MetadataServiceConfigurator(FfmpegConstant.Mp4Container, FfmpegConstant.Mp4FfmpegContainer, videoCodec, audioCodec);

            foreach (int resolution in resolutions)
            {
                serviceConfigurator.MaxWidth = resolution;
                serviceConfigurator.MaxHeight = resolution;
                StartEncode(serviceConfigurator, filePathes, String.Format("{0}_", resolution));
            }
        }

        [TestMethod]
        public void EncodeVideoWithAllResolutionsTest()
        {
            Directory.CreateDirectory(_destinationPath);
            var filePathes = Directory.GetFiles(_sourcePath);

            StartNewEncode(filePathes);
        }

        private void StartNewEncode(string[] filePathes)
        {
            int count = 0;
            var errorVideos = new List<string>();
            var allSw = new Stopwatch();
            var decodeSw = new Stopwatch();

            foreach (string filePath in filePathes)
            {
                allSw.Restart();
                Trace.WriteLine(String.Format("\n-------------Start decode file: {0}-------------\n", filePath));

                try
                {
                    var mediaInfo = new MediaInfoWrapper();
                    var metadataInfo = new VideoMetadataInfo(mediaInfo);
                    VideoMediaInfo metadata = metadataInfo.GetMetadata(filePath);
                    var encoder = new Encoder();

                    WriteFileInfo("N O T H I N G", metadata);

                    decodeSw.Restart();
                    encoder.EncodeVideo(metadata, filePath, _destinationPath);
                    allSw.Stop();
                    decodeSw.Stop();
                    WriteFinishProcess(decodeSw.Elapsed, allSw.Elapsed, 0);
                    count++;
                }
                catch (MediaFormatException ex)
                {
                    string errorMessage = String.Format("Error File:\t\t{0}.\nError Param:\t\t{1}", filePath, ex.Message);
                    Trace.WriteLine(String.Format("\n{0}", errorMessage));
                    errorVideos.Add(errorMessage);
                }
                catch (ExternalProcessException ex)
                {
                    string errorMessage = String.Format("Error File:\t\t{0}.\nFfmpeg return:\t\t{1}\n{2}", filePath, ex.Result, ex.Arguments);
                    Trace.WriteLine(errorMessage);
                    errorVideos.Add(errorMessage);
                }
                finally
                {
                    Trace.WriteLine(String.Format("\n-------------Finish decode file: {0}-------------\n", filePath));
                }
            }

            WriteFinishInfo(0, count, errorVideos);
        }

        private void StartEncode(MetadataServiceConfigurator serviceConfigurator, string[] filePathes, string prefix = "")
        {
            int count = 0;
            int excelStrCount = 2;
            double avgEncodeFps = 0;
            var errorVideos = new List<string>();
            var allSw = new Stopwatch();
            var decodeSw = new Stopwatch();
            Excel excel = CreateExcel();

            foreach (string filePath in filePathes)
            {
                allSw.Restart();
                Trace.WriteLine(String.Format("\n-------------Start decode file: {0}-------------\n", filePath));

                try
                {
                    string destinationFileName = GetDestinationFileName(filePath, prefix);
                    var mediaInfo = new MediaInfoWrapper();
                    var metadataInfo = new VideoMetadataInfo(mediaInfo);
                    VideoMediaInfo metadata = metadataInfo.GetMetadata(filePath);
                    var metadataService = new MetadataService(serviceConfigurator, metadata);
                    var stringBuilder = new FfmpegService(metadataService, filePath, _destinationPath, destinationFileName);
                    var ffmpeg = new Ffmpeg(stringBuilder);
                    string ffmpegString = stringBuilder.GetStringForEncoder();

                    WriteFileInfo(ffmpegString, metadata);

                    decodeSw.Restart();
                    ffmpeg.StartEncodeProcess();
                    allSw.Stop();
                    decodeSw.Stop();
                    WriteFinishProcess(decodeSw.Elapsed, allSw.Elapsed, ffmpeg.EncodeFps);

                    if (ffmpeg.EncodeFps > 0)
                    {
                        SetExcelLine(excel, metadata, filePath, excelStrCount, ffmpeg.EncodeFps);
                        excelStrCount++;
                    }

                    avgEncodeFps += ffmpeg.EncodeFps;
                    count++;
                }
                catch (MediaFormatException ex)
                {
                    string errorMessage = String.Format("Error File:\t\t{0}.\nError Param:\t\t{1}", filePath, ex.Message);
                    Trace.WriteLine(errorMessage);
                    errorVideos.Add(errorMessage);
                }
                catch (ExternalProcessException ex)
                {
                    string errorMessage = String.Format("Error File:\t\t{0}.\nFfmpeg return:\t\t{1}\n{2}", filePath, ex.Result, ex.Arguments);
                    Trace.WriteLine(errorMessage);
                    errorVideos.Add(errorMessage);
                }
                finally
                {
                    Trace.WriteLine(String.Format("\n-------------Finish decode file: {0}-------------\n", filePath));
                }
            }

            excel.SaveDocument(String.Format(@"{0}\{1}_{2}.xlsx", _destinationPath, serviceConfigurator.Container, DateTime.Now.ToString("u").Replace(":", "-")));
            excel.CloseDocument();

            avgEncodeFps = avgEncodeFps/count;
            WriteFinishInfo(avgEncodeFps, count, errorVideos);
        }

        private void StartScreenshot(MetadataServiceConfigurator serviceConfigurator, string[] filePathes)
        {
            int count = 0;
            double avgEncodeFps = 0;
            var errorVideos = new List<string>();
            var allSw = new Stopwatch();
            var decodeSw = new Stopwatch();
            foreach (string filePath in filePathes)
            {
                allSw.Restart();
                Trace.WriteLine(String.Format("\n-------------Start decode file: {0}-------------\n", filePath));

                try
                {
                    string destinationFileName = GetDestinationFileName(filePath);
                    var mediaInfo = new MediaInfoWrapper();
                    var metadataInfo = new VideoMetadataInfo(mediaInfo);
                    VideoMediaInfo metadata = metadataInfo.GetMetadata(filePath);
                    var metadataService = new MetadataService(serviceConfigurator, metadata);
                    var stringBuilder = new FfmpegService(metadataService, filePath, _destinationPath, destinationFileName);
                    var ffmpeg = new Ffmpeg(stringBuilder);
                    string ffmpegString = stringBuilder.GetStringForScreenshot();

                    WriteFileInfo(ffmpegString, metadata);

                    decodeSw.Restart();
                    ffmpeg.StartScreenshotProcess();
                    allSw.Stop();
                    decodeSw.Stop();
                    WriteFinishProcess(decodeSw.Elapsed, allSw.Elapsed, ffmpeg.EncodeFps);

                    avgEncodeFps += ffmpeg.EncodeFps;
                    count++;
                }
                catch (MediaFormatException ex)
                {
                    string errorMessage = String.Format("Error File:\t\t{0}.\nError Param:\t\t{1}", filePath, ex.Message);
                    Trace.WriteLine(errorMessage);
                    errorVideos.Add(errorMessage);
                }
                catch (ExternalProcessException ex)
                {
                    string errorMessage = String.Format("Error File:\t\t{0}.\nFfmpeg return:\t\t{1}\n{2}", filePath, ex.Result, ex.Arguments);
                    Trace.WriteLine(errorMessage);
                    errorVideos.Add(errorMessage);
                }
                finally
                {
                    Trace.WriteLine(String.Format("\n-------------Finish decode file: {0}-------------\n", filePath));
                }
            }

            avgEncodeFps = avgEncodeFps/count;
            WriteFinishInfo(avgEncodeFps, count, errorVideos);
        }

        private Excel CreateExcel()
        {
            var excel = new Excel();
            excel.NewDocument();
            excel.SetValue("A1", "Encode FPS");
            excel.SetValue("B1", "Duration Time");
            excel.SetValue("C1", "Video FPS");
            excel.SetValue("D1", "Video Key Frame");
            excel.SetValue("E1", "Width");
            excel.SetValue("F1", "Height");
            excel.SetValue("G1", "Container");
            excel.SetValue("H1", "Video Codec");
            excel.SetValue("I1", "Video Profile");
            excel.SetValue("J1", "Video Bitrate");
            excel.SetValue("K1", "Audio Codec");
            excel.SetValue("L1", "Audio Profile");
            excel.SetValue("M1", "Audio Bitrate");
            excel.SetValue("N1", "Audio Channel");
            excel.SetValue("O1", "File Size");
            excel.SetValue("P1", "File Path");

            return excel;
        }

        private void SetExcelLine(Excel excel, VideoMediaInfo metadata, string filePath, int numStr, double encodeFps)
        {
            excel.SetValue(String.Format("A{0}", numStr), encodeFps.ToString(CultureInfo.InvariantCulture));
            excel.SetValue(String.Format("B{0}", numStr), metadata.VideoDuration.ToString(CultureInfo.InvariantCulture));
            excel.SetValue(String.Format("C{0}", numStr), metadata.VideoFrameRate.ToString(CultureInfo.InvariantCulture));
            excel.SetValue(String.Format("D{0}", numStr), metadata.VideoFormatSettingsGOP.ToString(CultureInfo.InvariantCulture));

            excel.SetValue(String.Format("E{0}", numStr), metadata.VideoWidth.ToString(CultureInfo.InvariantCulture));
            excel.SetValue(String.Format("F{0}", numStr), metadata.VideoHeight.ToString(CultureInfo.InvariantCulture));

            excel.SetValue(String.Format("G{0}", numStr), metadata.GeneralFormat);
            excel.SetValue(String.Format("H{0}", numStr), metadata.VideoFormat);
            excel.SetValue(String.Format("I{0}", numStr), metadata.VideoFormatProfile);
            excel.SetValue(String.Format("J{0}", numStr), metadata.VideoBitRate.ToString(CultureInfo.InvariantCulture));

            excel.SetValue(String.Format("K{0}", numStr), metadata.AudioFormat);
            excel.SetValue(String.Format("L{0}", numStr), metadata.AudioFormatProfile);
            excel.SetValue(String.Format("M{0}", numStr), metadata.AudioBitRate.ToString(CultureInfo.InvariantCulture));
            excel.SetValue(String.Format("N{0}", numStr), metadata.AudioChannels.ToString(CultureInfo.InvariantCulture));

            excel.SetValue(String.Format("O{0}", numStr), metadata.GeneralFileSize.ToString(CultureInfo.InvariantCulture));

            excel.SetValue(String.Format("P{0}", numStr), filePath);
        }

        private void WriteFinishInfo(double avgEncodeFps, int quantityVideos, List<string> errorVideos)
        {
            Trace.WriteLine("****************************************************************************");
            Trace.WriteLine("*                                                                          *");
            Trace.WriteLine("*  ||||||||||  ||     ||  |||||||||      |||||||||  |||    ||  ||||||      *");
            Trace.WriteLine("*      ||      ||     ||  ||             ||         ||||   ||  ||    ||    *");
            Trace.WriteLine("*      ||      |||||||||  |||||||||      |||||||||  || ||  ||  ||     ||   *");
            Trace.WriteLine("*      ||      ||     ||  ||             ||         ||  || ||  ||     ||   *");
            Trace.WriteLine("*      ||      ||     ||  ||             ||         ||   ||||  ||     ||   *");
            Trace.WriteLine("*      ||      ||     ||  |||||||||      |||||||||  ||    |||  |||||||     *");
            Trace.WriteLine("*                                                                          *");
            Trace.WriteLine("****************************************************************************");
            Trace.WriteLine(String.Format("\nVideos Encoded:    {0}", quantityVideos));
            Trace.WriteLine(String.Format("Average Encode FPS:  {0}", avgEncodeFps));

            Trace.WriteLine(String.Format("\nFailed Videos:     {0}", errorVideos.Count));
            int count = 0;
            foreach (string errorVideo in errorVideos)
            {
                Trace.WriteLine(String.Format("\n----------------------|{0}|----------------------", count));
                Trace.WriteLine(errorVideo);
                Trace.WriteLine(String.Format("----------------------|{0}|----------------------", count));
                count++;
            }
        }

        private void WriteFinishProcess(TimeSpan decodeTime, TimeSpan allTime, double encodeFps)
        {
            Trace.WriteLine("\n!!!Finish Process!!!");
            Trace.WriteLine(String.Format("All Time:            {0}", allTime));
            Trace.WriteLine(String.Format("Encode Time:         {0}", decodeTime));
            Trace.WriteLine(String.Format("Encode FPS:          {0}", encodeFps));
        }

        private void WriteFileInfo(string ffmpegString, VideoMediaInfo metadata)
        {
            Trace.WriteLine(String.Format("Width:               {0}", metadata.VideoWidth));
            Trace.WriteLine(String.Format("Width:               {0}", metadata.VideoHeight));
            Trace.WriteLine(String.Format("File Size:           {0} kByte", metadata.GeneralFileSize/1024));
            Trace.WriteLine(String.Format("Container:           {0}", metadata.GeneralFormat));
            Trace.WriteLine(String.Format("Video Codec:         {0}", metadata.VideoFormat));
            Trace.WriteLine(String.Format("Video Profile:       {0}", metadata.VideoFormatProfile));
            Trace.WriteLine(String.Format("Video Bitrate:       {0} kBit/s", metadata.VideoBitRate/1000));
            Trace.WriteLine(String.Format("Audio Codec:         {0}", metadata.AudioFormat));
            Trace.WriteLine(String.Format("Audio Profile:       {0}", metadata.AudioFormatProfile));
            Trace.WriteLine(String.Format("Audio Channel(s):    {0}", metadata.AudioChannels));
            Trace.WriteLine(String.Format("Audio Bitrate:       {0} kBit/s", metadata.AudioBitRate/1000));
            Trace.WriteLine(String.Format("Duration:            {0}", TimeSpan.FromMilliseconds(metadata.VideoDuration)));
            Trace.WriteLine(String.Format("Video FPS:           {0}", metadata.VideoFrameRate));
            Trace.WriteLine(String.Format("Video Key Frame:     {0}", metadata.VideoFormatSettingsGOP));
            Trace.WriteLine(String.Format("Ffmpeg String:       {0}", ffmpegString));
        }

        private string GetDestinationFileName(string filePath, string str = "")
        {
            var fileInfo = new FileInfo(filePath);
            return String.Format("{0}{1}", str, fileInfo.Name);
        }
    }
}