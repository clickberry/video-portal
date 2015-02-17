using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.LocalFileSystem;
using Portal.BackEnd.Encoder.Settings;
using Wrappers.Interface;

namespace Portal.BackEnd.Encoder.Test.LocalFileSystemTests
{
    [TestClass]
    public class TempFileManagerTest
    {
        const string TempPath = "tempPath";
        const string NewTempPath = "newTempPath";
        const string TempFilePath = "tempFilePath";

        private Mock<IFileSystemWrapper> _fileSystem;
        private TempFileManager _tempFileManager;

        [TestInitialize]
        public void Initialize()
        {
            _fileSystem = new Mock<IFileSystemWrapper>();
            _tempFileManager = new TempFileManager(_fileSystem.Object);

            _fileSystem.Setup(m => m.GetTempPath()).Returns(TempPath);
            _fileSystem.Setup(m => m.PathCombine(TempPath, Environment.MachineName)).Returns(NewTempPath);
        }

        [TestMethod]
        public void GetOriginalTempFilePathTest()
        {
            //Arrange
            _fileSystem.Setup(m => m.PathCombine(NewTempPath, TempFileManager.OriginalFile)).Returns(TempFilePath);

            //Act
            var filePath = _tempFileManager.GetOriginalTempFilePath();

            //Assert
            Assert.AreEqual(TempFilePath, filePath);

            _fileSystem.Verify(m => m.CreateDirectory(NewTempPath), Times.Once());
        }

        [TestMethod]
        public void GetEncodingTempFilePathTest()
        {
            //Arrange
            _fileSystem.Setup(m => m.PathCombine(NewTempPath, TempFileManager.EncodingFile)).Returns(TempFilePath);

            //Act
            var filePath = _tempFileManager.GetEncodingTempFilePath();

            //Assert
            Assert.AreEqual(TempFilePath, filePath);

            _fileSystem.Verify(m => m.CreateDirectory(NewTempPath), Times.Once());
        }

        [TestMethod]
        public void DeleteAllTempFilesTest()
        {
            //Arrange
            _fileSystem.Setup(m => m.DirectoryExists(It.IsAny<string>())).Returns(true);

            //Act
            _tempFileManager.DeleteAllTempFiles();

            //Assert
            _fileSystem.Verify(m=>m.DeleteDirectory(NewTempPath), Times.Once());
        }

        [TestMethod]
        public void ExistsEncodingFileTest()
        {
            //Arrange
            _fileSystem.Setup(m => m.PathCombine(NewTempPath, TempFileManager.EncodingFile)).Returns(TempFilePath);
            _fileSystem.Setup(m => m.FileExists(TempFilePath)).Returns(true);

            //Act
            var isExist = _tempFileManager.ExistsEncodingFile();

            //Assert
            Assert.AreEqual(true, isExist);
        }

        [TestMethod]
        public void NotExistsEncodingFileTest()
        {
            //Arrange
            _fileSystem.Setup(m => m.PathCombine(NewTempPath, TempFileManager.EncodingFile)).Returns(TempFilePath);
            _fileSystem.Setup(m => m.FileExists(TempFilePath)).Returns(false);

            //Act
            var isExist = _tempFileManager.ExistsEncodingFile();

            //Assert
            Assert.AreEqual(false, isExist);
        }
    }
}
