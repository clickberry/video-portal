using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routines.Url;

namespace Routines.Tests
{
    [TestClass]
    public class UrlShortenerTests
    {
        [TestMethod]
        public void TestCompression()
        {
            Guid expected = Guid.NewGuid();

            var shortener = new UrlShortener();

            string compressed = shortener.Compress(expected);
            Guid actual = shortener.Decompress(compressed);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCompressionManyTimes()
        {
            Parallel.For(0, (int) 1e+8, _ => TestCompression());
        }
    }
}