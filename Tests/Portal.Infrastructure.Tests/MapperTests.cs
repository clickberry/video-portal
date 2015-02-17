using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Infrastructure.Mapper;

namespace Portal.Infrastructure.Tests
{
    [TestClass]
    public class MapperTests
    {
        private const string ConstValueString1 = "String1";
        private const string ConstValueString2 = "String2";
        private const int ConstValueInt = 3;
        private const double ConstValueDouble = .5;

        [TestMethod]
        public void TestMappingOneToOne()
        {
            // Arrange
            IMapper mapper = new Mapper.Mapper();
            mapper.Register<Class1From, ClassTo>(p => new ClassTo
                {
                    ValueInt = p.ValueInt,
                    ValueString = p.ValueString
                });

            var classFrom = new Class1From
                {
                    ValueInt = ConstValueInt,
                    ValueString = ConstValueString1
                };

            // Act
            ClassTo classTo = mapper.Map<Class1From, ClassTo>(classFrom);

            // Assert
            Assert.IsNotNull(classTo);
            Assert.AreEqual(classTo.ValueInt, ConstValueInt);
            Assert.AreEqual(classTo.ValueString, ConstValueString1);
        }

        [TestMethod]
        public void TestMappingTwoToOne()
        {
            // Arrange
            IMapper mapper = new Mapper.Mapper();
            mapper.Register<Class1From, Class2From, ClassTo>((p, q) => new ClassTo
                {
                    ValueInt = p.ValueInt,
                    ValueString = p.ValueString + q.ValueString,
                    ValueDouble = q.ValueDouble
                });

            var classFrom1 = new Class1From
                {
                    ValueInt = ConstValueInt,
                    ValueString = ConstValueString1
                };

            var classFrom2 = new Class2From
                {
                    ValueDouble = ConstValueDouble,
                    ValueString = ConstValueString2
                };

            // Act
            ClassTo classTo = mapper.Map<Class1From, Class2From, ClassTo>(classFrom1, classFrom2);

            // Assert
            Assert.IsNotNull(classTo);
            Assert.AreEqual(classTo.ValueInt, 3);
            Assert.AreEqual(classTo.ValueDouble, ConstValueDouble);
            Assert.AreEqual(classTo.ValueString, ConstValueString1 + ConstValueString2);
        }

        #region Nested type: Class1From

        private class Class1From
        {
            public string ValueString { get; set; }

            public int ValueInt { get; set; }
        }

        #endregion

        #region Nested type: Class2From

        private class Class2From
        {
            public string ValueString { get; set; }

            public double ValueDouble { get; set; }
        }

        #endregion

        #region Nested type: ClassTo

        private class ClassTo
        {
            public string ValueString { get; set; }

            public double ValueDouble { get; set; }

            public int ValueInt { get; set; }
        }

        #endregion
    }
}