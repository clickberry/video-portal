using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Portal.BackEnd.Encoder.Interface;
using Portal.BackEnd.Encoder.StringBuilder;
using Portal.Domain.BackendContext.Entity.Base;

namespace Portal.BackEnd.Encoder.Test.StringBuilderTests
{
    [TestClass]
    public class EncodeStringBuilderBaseTest
    {
        [TestMethod]
        public void GetContentTypeTest()
        {
            //Arrange
            const string contentType = "contentType";
            var encodeData = new Mock<IEncodeData>();
            var stringBuilder = new TestStringBuilder(encodeData.Object, It.IsAny<ITempFileManager>());

            encodeData.Setup(p => p.ContentType).Returns(contentType);

            //Act
            var result = stringBuilder.GetContentType();

            //Assert
            Assert.AreEqual(contentType,result);
        }
    }

    internal class TestStringBuilder:EncodeStringBuilderBase
    {
        public TestStringBuilder(IEncodeData encodeData, ITempFileManager tempFileManager)
            : base(encodeData)
        {
            
        }
    }
}
