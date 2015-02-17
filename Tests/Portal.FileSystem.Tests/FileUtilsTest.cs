using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.FileSystem.Infrastructure;
using System.Diagnostics;
using System;
using System.IO;

namespace Portal.FileSystem.Tests
{
    [TestClass]
    public class FileUtilsTest
    {
        [TestMethod]
        public void TestGetFileHash()
        {
            string hashString = FileUtils.GetFileHash("html.avsx");

            for (int i = 0; i < 100000; i++)
            {
                string newHash = FileUtils.GetFileHash("html.avsx");
                Assert.AreEqual(newHash, hashString);
            }
        }
    }
}