using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Ffmpeg;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.Settings;
using Portal.Domain.BackendContext.Enum;
using Portal.SLL.Abstract;
using Wrappers;
using Portal.Domain.FileContext;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Test.FfmpegTests
{
    [TestClass]
    public class FfmpegProcessTest
    {
        const string Arguments = "arguments";

        private readonly CancellationTokenSourceWrapper _tokenSource = new CancellationTokenSourceWrapper();
        private readonly Action<string> _processData = (s) => { };
        private Mock<IProcessAsync> _process;
        private Mock<ITempFileManager> _tempFileManager;
        private FfmpegProcess _ffmpeg;

        [TestInitialize]
        public void Initialize()
        {
            _process = new Mock<IProcessAsync>();
            _tempFileManager = new Mock<ITempFileManager>();
            _ffmpeg = new FfmpegProcess(_process.Object, _tempFileManager.Object);
        }

        [TestMethod]
        public void FfmpegSuccessfulFinishTest()
        {
            //Arrange
            _tempFileManager.Setup(m => m.ExistsEncodingFile()).Returns(true);
            _process.Setup(m => m.Start(Arguments, _processData, _tokenSource.Token)).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    tcs.SetResult(null);
                    return tcs.Task;
                });

            //Act
            var result = _ffmpeg.Start(Arguments, _tokenSource, _processData).Result;

            //Assert
            Assert.AreEqual(false, _tokenSource.IsCancellationRequested);
            Assert.AreEqual(EncoderState.Completed, result.EncoderState);
            Assert.AreEqual(String.Empty, result.ErrorMessage);
        }

        [TestMethod]
        public void FfmpegFailedFinishTest()
        {
            //Arrange
            const string errorMessage = "errorMessage";

            _process.Setup(m => m.Start(Arguments, _processData, _tokenSource.Token)).Returns(() =>
                {
                    var tcs = new TaskCompletionSource<object>();
                    tcs.SetException(new Exception(errorMessage));
                    return tcs.Task;
                });

            //Act
            var result = _ffmpeg.Start(Arguments, _tokenSource, _processData).Result;

            //Assert
            Assert.AreEqual(true, _tokenSource.IsCancellationRequested);
            Assert.AreEqual(EncoderState.Failed, result.EncoderState);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [TestMethod]
        public void FfmpegCancelTest()
        {
            //Arrange
            _process.Setup(m => m.Start(Arguments, _processData, _tokenSource.Token)).Returns(() =>
            {
                var tcs = new TaskCompletionSource<object>();
                tcs.SetCanceled();
                return tcs.Task;
            });

            //Act
            var result = _ffmpeg.Start(Arguments, _tokenSource, _processData).Result;

            //Assert
            Assert.AreEqual(true, _tokenSource.IsCancellationRequested);
            Assert.AreEqual(EncoderState.Cancelled, result.EncoderState);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [TestMethod]
        public void FfmpegNotCreateEncodingFileTest()
        {
            //Arrange
            _process.Setup(m => m.Start(Arguments, _processData, _tokenSource.Token)).Returns(() =>
            {
                var tcs = new TaskCompletionSource<object>();
                tcs.SetResult(null);
                return tcs.Task;
            });

            //Act
            var result = _ffmpeg.Start(Arguments, _tokenSource, _processData).Result;

            //Assert
            Assert.AreEqual(true, _tokenSource.IsCancellationRequested);
            Assert.AreEqual(EncoderState.Failed, result.EncoderState);
            Assert.AreEqual("Output file was not created.", result.ErrorMessage);
        }
    }
}
