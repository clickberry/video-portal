using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BLL.Concrete.Infrastructure;
using Portal.Domain.Admin;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Tests
{
    [TestClass]
    public class LambdaCreatorShould
    {
        [TestMethod]
        public void CreateWhereContainsLambdaSuccessfully()
        {
            // Arrange
            var lambda = new LambdaCreator();
            var contains = new ContainsFilterRules {MemberName = "Prop", MemberValue = "2a"};
            var pocos = Enumerable.Range(1, 10).Select(i => new Poco {Prop = string.Format("La {0}a la", i)}).ToList();
            Poco expected = pocos[1];

            // Act
            var mce = lambda.CreateContainsLambda<Poco>(contains.MemberName, contains.MemberValue);


            var res = pocos.AsQueryable().Where(mce);
            var res2 = pocos.Where(p => p.Prop.Contains(contains.MemberValue)).ToList();

            // Assert
            Assert.AreEqual(1, res.Count());
            Assert.AreEqual(expected.Prop, res.First().Prop);
            Assert.AreEqual(res2.First().Prop, res.First().Prop);
            
        }

        [TestMethod]
        public void ThrowBadRequestIfQueriedPropertyDoesNotExist()
        {
            // Arrange
            var lambda = new LambdaCreator();
            var contains = new ContainsFilterRules { MemberName = "Non-ExistentProp", MemberValue = "15" };
            var pocos = Enumerable.Range(1, 10).Select(i => new Poco { Prop = string.Format("La {0}a la", i) }).ToList();

            // Act
            try
            {
                lambda.CreateContainsLambda<Poco>(contains.MemberName, contains.MemberValue);
            // Assert
                Assert.Fail("Should throw exception");
            }
            catch (BadRequestException exception)
            {

            }
            catch (Exception e)
            {
                Assert.Fail("Should throw bad request exception");
            }
        }

        [TestMethod]
        public void ThrowBadRequestIfQueriedPropertyIsNotAString()
        {
            // Arrange
            var lambda = new LambdaCreator();
            var contains = new ContainsFilterRules { MemberName = "IntProp", MemberValue = "15" };
            var pocos = Enumerable.Range(1, 10).Select(i => new Poco { Prop = string.Format("La {0}a la", i) }).ToList();

            // Act
            try
            {
                lambda.CreateContainsLambda<Poco>(contains.MemberName, contains.MemberValue);
            // Assert
                Assert.Fail("Should throw exception");
            }
            catch (BadRequestException exception)
            {

            }
            catch (Exception e)
            {
                Assert.Fail("Should throw bad request exception");
            }
        }

        [TestMethod]
        public void SkipElementOfCollectionIfItsQueriedPropertyIsNull()
        {
            // Arrange
            var lambda = new LambdaCreator();
            var contains = new ContainsFilterRules { MemberName = "Prop", MemberValue = "la" };
            var pocos = Enumerable.Range(1, 10).Select(i => new Poco { Prop = string.Format("La {0}a la", i) }).ToList();
            pocos.Add(new Poco()); // Prop = null.

            // Act
            var mce = lambda.CreateContainsLambda<Poco>(contains.MemberName, contains.MemberValue);
            var result = pocos.AsQueryable().Where(mce).ToList();
            
            // Assert
            Assert.AreEqual(pocos.Count - 1, result.Count);
        }

        [TestMethod]
        public void CreateWhereEqualsLambdaSuccessfully()
        {
            // Arrange
            const string propertyName = "IntProp";
            const int propertyValue = 2;
            var entities = Enumerable.Range(1, 2).SelectMany(i => Enumerable.Repeat(new Poco{IntProp = i}, 10)).ToList();
            var lambdaCreator = new LambdaCreator();
            
            // Act
            Expression<Func<Poco, bool>> equalsLambda = lambdaCreator.CreateEqualsLambda<Poco>(propertyName, propertyValue);
            var result = entities.AsQueryable().Where(equalsLambda).ToList();

            // Assert
            Assert.AreEqual(10, result.Count);
            Assert.IsTrue(result.All(i => i.IntProp == 2));

        }

        [TestMethod]
        public void PerformCaseInsensitiveSearchSuccessfully()
        {
            // Arrange
            var lambda = new LambdaCreator();
            var contains = new ContainsFilterRules { MemberName = "Prop", MemberValue = "10A" };
            var pocos = Enumerable.Range(1, 10).Select(i => new Poco { Prop = string.Format("La {0}a la", i), IntProp = i}).ToList();

            // Act
            var mce = lambda.CreateContainsLambda<Poco>(contains.MemberName, contains.MemberValue);
            var result = pocos.AsQueryable().Where(mce).ToList();

            // Assert
            Assert.AreEqual(10, result.Single().IntProp);
        }

        public class Poco
        {
            public string Prop { get; set; }
            public int IntProp { get; set; }
        }
    }
}
