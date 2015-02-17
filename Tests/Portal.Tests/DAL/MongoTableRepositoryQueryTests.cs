// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Portal.DAL.Azure.Context;
using Portal.Exceptions.CRUD;
using Portal.Tests.Attributes;
using Xunit;

namespace Portal.Tests.DAL
{
    public sealed class MongoTableRepositoryQueryTests
    {
        private readonly MongoTableRepository<Poco> _repository;

        public MongoTableRepositoryQueryTests()
        {
            _repository = new MongoTableRepository<Poco>(new MongoUrl("mongodb://localhost/tests"));
        }

        [IntegrationFact]
        public async Task First()
        {
            // Arrange
            List<Poco> entities = Enumerable.Range(0, 5).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            Poco result = await _repository.FirstAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities[0].Id, result.Id);
            Assert.Equal(entities[0].LongValue, result.LongValue);
            Assert.Equal(entities[0].StringValue, result.StringValue);
        }

        [IntegrationFact]
        public async Task FirstWithPredicate()
        {
            // Arrange
            const int id = 2;
            List<Poco> entities = Enumerable.Range(0, 5).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            Poco result = await _repository.FirstAsync(p => p.LongValue == id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities[id].Id, result.Id);
            Assert.Equal(entities[id].LongValue, result.LongValue);
            Assert.Equal(entities[id].StringValue, result.StringValue);
        }

        [IntegrationFact]
        public async Task FirstWithNotFound()
        {
            // Arrange
            Exception exception = null;

            // Act
            try
            {
                await _repository.FirstAsync();
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<NotFoundException>(exception);
        }

        [IntegrationFact]
        public async Task FirstWithPredicateWithNotFound()
        {
            // Arrange
            Exception exception = null;

            // Act
            try
            {
                await _repository.FirstAsync(p => p.LongValue == 0);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<NotFoundException>(exception);
        }

        [IntegrationFact]
        public async Task FirstOrDefault()
        {
            // Arrange
            List<Poco> entities = Enumerable.Range(0, 5).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            Poco result = await _repository.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities[0].Id, result.Id);
            Assert.Equal(entities[0].LongValue, result.LongValue);
            Assert.Equal(entities[0].StringValue, result.StringValue);
        }

        [IntegrationFact]
        public async Task FirstOrDefaultWithPredicate()
        {
            // Arrange
            const int id = 2;
            List<Poco> entities = Enumerable.Range(0, 5).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            Poco result = await _repository.FirstOrDefaultAsync(p => p.LongValue == id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities[id].Id, result.Id);
            Assert.Equal(entities[id].LongValue, result.LongValue);
            Assert.Equal(entities[id].StringValue, result.StringValue);
        }

        [IntegrationFact]
        public async Task FirstOrDefaultWithNull()
        {
            // Arrange
            Exception exception = null;
            Poco result = null;

            // Act
            try
            {
                result = await _repository.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.Null(result);
            Assert.Null(exception);
        }

        [IntegrationFact]
        public async Task FirstOrDefaultWithPredicateWithNull()
        {
            // Arrange
            Exception exception = null;
            Poco result = null;

            // Act
            try
            {
                result = await _repository.FirstOrDefaultAsync(p => p.LongValue == 0);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.Null(result);
            Assert.Null(exception);
        }

        [IntegrationFact]
        public async Task Single()
        {
            // Arrange
            List<Poco> entities = Enumerable.Range(0, 1).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            Poco result = await _repository.SingleAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities[0].Id, result.Id);
            Assert.Equal(entities[0].LongValue, result.LongValue);
            Assert.Equal(entities[0].StringValue, result.StringValue);
        }

        [IntegrationFact]
        public async Task SingleWithPredicate()
        {
            // Arrange
            const int id = 2;
            List<Poco> entities = Enumerable.Range(0, 5).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            Poco result = await _repository.SingleAsync(p => p.LongValue == id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities[id].Id, result.Id);
            Assert.Equal(entities[id].LongValue, result.LongValue);
            Assert.Equal(entities[id].StringValue, result.StringValue);
        }

        [IntegrationFact]
        public async Task SingleWithNotFound()
        {
            // Arrange
            Exception exception = null;

            // Act
            try
            {
                await _repository.SingleAsync();
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<NotFoundException>(exception);
        }

        [IntegrationFact]
        public async Task SingleWithPredicateWithNotFound()
        {
            // Arrange
            Exception exception = null;

            // Act
            try
            {
                await _repository.SingleAsync(p => p.LongValue == 0);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<NotFoundException>(exception);
        }

        [IntegrationFact]
        public async Task SingleOrDefault()
        {
            // Arrange
            List<Poco> entities = Enumerable.Range(0, 1).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            Poco result = await _repository.SingleOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities[0].Id, result.Id);
            Assert.Equal(entities[0].LongValue, result.LongValue);
            Assert.Equal(entities[0].StringValue, result.StringValue);
        }

        [IntegrationFact]
        public async Task SingleOrDefaultWithPredicate()
        {
            // Arrange
            const int id = 2;
            List<Poco> entities = Enumerable.Range(0, 5).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            Poco result = await _repository.SingleOrDefaultAsync(p => p.LongValue == id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities[id].Id, result.Id);
            Assert.Equal(entities[id].LongValue, result.LongValue);
            Assert.Equal(entities[id].StringValue, result.StringValue);
        }

        [IntegrationFact]
        public async Task SingleOrDefaultWithNull()
        {
            // Arrange
            Exception exception = null;
            Poco result = null;

            // Act
            try
            {
                result = await _repository.SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.Null(result);
            Assert.Null(exception);
        }

        [IntegrationFact]
        public async Task SingleOrDefaultWithPredicateWithNull()
        {
            // Arrange
            Exception exception = null;
            Poco result = null;

            // Act
            try
            {
                result = await _repository.SingleOrDefaultAsync(p => p.LongValue == 0);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.Null(result);
            Assert.Null(exception);
        }

        [IntegrationFact]
        public async Task TakeList()
        {
            // Arrange
            const int count = 5;
            const int take = 2;
            List<Poco> entities = Enumerable.Range(0, count).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            List<Poco> results = await _repository.TakeAsync(take);

            // Assert
            Assert.Equal(take, results.Count);
        }

        [IntegrationFact]
        public async Task TakeWithPredicate()
        {
            // Arrange
            const int count = 5;
            const int id = 1;
            const int take = 2;
            List<Poco> entities = Enumerable.Range(0, count).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            List<Poco> results = await _repository.TakeAsync(p => p.LongValue > id, take);

            // Assert
            Assert.Equal(take, results.Count);
            Assert.True(results.All(p => p.LongValue > id));
        }

        [IntegrationFact]
        public async Task ToList()
        {
            // Arrange
            const int count = 5;
            List<Poco> entities = Enumerable.Range(0, count).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            List<Poco> results = await _repository.ToListAsync();

            // Assert
            Assert.Equal(count, results.Count);
        }

        [IntegrationFact]
        public async Task ToListWithPredicate()
        {
            // Arrange
            const int count = 5;
            const int id = 2;
            List<Poco> entities = Enumerable.Range(0, count).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            List<Poco> results = await _repository.ToListAsync(p => p.LongValue > id);

            // Assert
            Assert.True(results.All(p => p.LongValue > id));
        }

        [IntegrationFact]
        public async Task AsEnumerable()
        {
            // Arrange
            const int count = 5;
            List<Poco> entities = Enumerable.Range(0, count).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            List<Poco> results = _repository.AsEnumerable().ToList();

            // Assert
            Assert.Equal(count, results.Count);
        }

        [IntegrationFact]
        public async Task AsEnumerableWithPredicate()
        {
            // Arrange
            const int count = 5;
            const int id = 2;
            List<Poco> entities = Enumerable.Range(0, count).Select(p => new Poco
            {
                LongValue = p,
                StringValue = p.ToString(CultureInfo.InvariantCulture)
            }).ToList();

            await _repository.AddAsync(entities);

            // Act
            List<Poco> results = _repository.AsEnumerable(p => p.LongValue > id).ToList();

            // Assert
            Assert.True(results.All(p => p.LongValue > id));
        }
    }
}