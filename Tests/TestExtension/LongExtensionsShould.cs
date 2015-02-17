using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestExtension
{
    [TestClass]
    public class LongExtensionsShould
    {
        [TestMethod]
        public void DisplayGigabytes()
        {
            // Arrange
            long length = 1024L*1024*1024;
            const string expected = "1Gb";

            // Act
            string result = length.PrintLong().Replace(" ", "");

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DisplayMegabytes()
        {
            // Arrange
            const long length = 1024L*1024;
            const string expected = "1Mb";

            // Act
            string result = length.PrintLong().Replace(" ", "");

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DisplayKilobytes()
        {
            // Arrange
            const long length = 1024L;
            const string expected = "1Kb";

            // Act
            string result = length.PrintLong().Replace(" ", "");

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DisplayBytes()
        {
            // Arrange
            const long length = 32L;
            const string expected = "32b";

            // Act
            string result = length.PrintLong().Replace(" ", "");

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DisplayFractionOfMegabyte()
        {
            // Arrange
            const long length = 32L * 1024L * 1024L + 256L * 1024L;
            const string expected = "32.3Mb";

            // Act
            string result = length.PrintLong().Replace(" ", "");

            // Assert
            Assert.AreEqual(expected, result);
        }
    }

    public static class LongExtensions
    {
        private enum Sizes
        {
            Zero = 0,
            Byte, 
            Kilo,
            Mega,
            Giga
        }

        public static string PrintLong(this long number)
        {
            Sizes size = GetSize(number);
            string qualifier = GetQualifier(size);

            switch (size)
            {
                case Sizes.Byte:
                    return string.Format("{0}{1}", number, qualifier);
                case Sizes.Kilo:
                    return string.Format("{0:0}{1}", number / 1024.0, qualifier);
                case Sizes.Mega:
                    return string.Format("{0:0.#}{1}", number / (1024.0 * 1024.0), qualifier);
                case Sizes.Giga:
                    return string.Format("{0:0.##}{1}", number / (1024.0 * 1024.0 * 1024.0), qualifier);
            }
            return "0";
        }

        private static string GetQualifier(Sizes size)
        {
            switch (size)
            {
                case Sizes.Kilo:
                    return "Kb";
                case Sizes.Mega:
                    return "Mb";
                case Sizes.Giga:
                    return "Gb";
                case Sizes.Byte:
                    return "b";
            }
            return "";
        }

        private static Sizes GetSize(long number)
        {
            int i = 0;
            for ( ;number > 0; i++)
            {
                number /= 1024L;
            }
            return (Sizes) i;
        }
    }
}