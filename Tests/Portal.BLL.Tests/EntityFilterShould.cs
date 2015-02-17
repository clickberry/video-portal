using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BLL.Abstract;
using Portal.BLL.Concrete.Infrastructure;
using Portal.DAL.Entities.Table;
using Portal.Domain.Admin;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Tests
{
    [TestClass]
    public class EntityFilterShould
    {
        private readonly LambdaCreator _lambdaCreator;

        public EntityFilterShould()
        {
            _lambdaCreator = new LambdaCreator();
        }

        [TestMethod]
        public void TakeLastElementSuccessfully()
        {
            // Arrange
            const string expectedName = "Expected";
            var filterRules = new OrderByFilterRules()
                            {
                                Skip = 2,
                                Take = 1
                            };
            var entities = new List<UserProfileEntity>
                                                   {
                                                       new UserProfileEntity(),
                                                       new UserProfileEntity(),
                                                       new UserProfileEntity{UserName = expectedName}
                                                   };

            IFilter filter = new EntityFilter(_lambdaCreator);
            
            // Act
            var result = filter.FilterWithOrder(entities, filterRules);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedName, result[0].UserName);

        }

        [TestMethod]
        public void Return0ElementsIfTakeIsLesserThan1()
        {
            // Arrange
            var invalidFilter = new OrderByFilterRules() {Take = -1};
            var entities = Enumerable.Range(1, 30)
                           .Select(_ => new UserProjectEntity())
                           .ToList();
            IFilter entityFilter = new EntityFilter(_lambdaCreator);

            // Act
            var res = entityFilter.FilterWithOrder(entities, invalidFilter);

            Assert.AreEqual(0, res.Count);

        }

        [TestMethod]
        public void OrderByDescendingSuccessfully()
        {
            // Arrange
            const int max = 30;
            var filterRules = new OrderByFilterRules() {Take = 5, Orderby = "Created"};
            var entities = Enumerable.Range(1, max)
                           .Select(value => new UserProjectEntity
                                                {
                                                    Created = DateTime.MinValue + TimeSpan.FromDays(value),
                                                    Name = value.ToString(CultureInfo.InvariantCulture)
                                                })
                           .ToList();
            IFilter filter = new EntityFilter(_lambdaCreator);

            // Act
            var result = filter.FilterWithOrder(entities, filterRules);
            string firstElementName = result.First().Name;


            // Assert
            Assert.AreEqual(max.ToString(), firstElementName);

        }

        [TestMethod]
        public void ReturnResultEvenIfRangeIsBiggerThanLength()
        {
            // Arrange
            const int max = 15;
            const int skip = 10;
            var filterRules = new OrderByFilterRules() { Take = 10, Orderby = "Created", Skip = skip};
            var entities = Enumerable.Range(1, max)
                           .Select(value => new UserProjectEntity
                           {
                               Created = DateTime.MinValue + TimeSpan.FromDays(value),
                               Name = value.ToString(CultureInfo.InvariantCulture)
                           })
                           .ToList();
            IFilter filter = new EntityFilter(_lambdaCreator);


            // Act
            var result = filter.FilterWithOrder(entities, filterRules);

            // Assert
            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public void FilterByContainsSuccessfully()
        {
            // Arrange
            const int max = 30;
            var contains = new ContainsFilterRules {MemberName = "Description", MemberValue = max.ToString(CultureInfo.InvariantCulture)};

            var entities = Enumerable.Range(1, max)
                           .Select(value => new UserProjectEntity
                           {
                               Created = DateTime.MaxValue - TimeSpan.FromDays(value),
                               Name = value.ToString(CultureInfo.InvariantCulture),
                               Description = (value > 4 && value < 10) 
                                               ? max.ToString(CultureInfo.InvariantCulture) 
                                               : "Other description"
                           })
                           .ToList();
            IFilter filter = new EntityFilter(_lambdaCreator);

            // Act
            var result = filter.FilterWithContains(entities, contains);
            string firstElementName = result.First().Name;


            // Assert
            Assert.IsTrue(result.All(project => project.Description == max.ToString()));
            Assert.AreEqual("5", firstElementName);
            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public void ParseObjectExpressionsSuccessfully()
        {
            var rules = new ContainsFilterRules {MemberName = "value", MemberValue = "10"};
            var entities = Enumerable.Range(1, 10).Select(i => new {id = i, value = "lala" + i.ToString(CultureInfo.InvariantCulture)}).ToList();
            var lamdaCreator = new LambdaCreator();
            var filter = new EntityFilter(lamdaCreator);

            var res = filter.FilterWithContains(entities, rules);

            Assert.AreEqual(1, res.Count);
            Assert.AreEqual(10, res[0].id);

        }

        [TestMethod]
        public void FilterWithEqualsSuccessfully()
        {
            // Arrange
            const string propertyName = "IntProp";
            var equals = new EqualsFilterRules<int>{MemberName = propertyName, MemberValue = 2};
            var entities = Enumerable.Range(1, 2).SelectMany(i => Enumerable.Repeat(new LambdaCreatorShould.Poco { IntProp = i }, 10)).ToList();
            var lambdaCreator = new LambdaCreator();
            IFilter entityFilter = new EntityFilter(lambdaCreator);

            // Act
            List<LambdaCreatorShould.Poco> res = entityFilter.FilterWithEquals(entities, @equals);
            
            // Assert
            Assert.AreEqual(10, res.Count);
            Assert.IsTrue(res.All(elem => elem.IntProp == 2));

        }

    }
}
