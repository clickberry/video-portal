// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Portal.DAL.Azure.Context;
using Portal.Exceptions.CRUD;
using Portal.Tests.Attributes;
using Xunit;

namespace Portal.Tests.DAL
{
    public sealed class MongoTableRepositoryCudTests : IDisposable
    {
        private readonly MongoTableRepository<Poco> _repository;

        public MongoTableRepositoryCudTests()
        {
            _repository = new MongoTableRepository<Poco>(new MongoUrl("mongodb://localhost/tests"));
        }

        public void Dispose()
        {
        }

        [IntegrationFact]
        public async Task AddEntity()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";
            var entity = new Poco
            {
                LongValue = longValue,
                StringValue = stringValue
            };

            // Act
            Poco result = await _repository.AddAsync(entity);

            // Assert
            Assert.Equal(longValue, result.LongValue);
            Assert.Equal(stringValue, result.StringValue);
            Assert.NotEqual(null, result.Id);
        }

        [IntegrationFact]
        public async Task AddEntityWithConflict()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";
            var entity = new Poco
            {
                LongValue = longValue,
                StringValue = stringValue
            };

            await _repository.AddAsync(entity);
            Exception exception = null;

            // Act
            try
            {
                await _repository.AddAsync(entity);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ConflictException>(exception);
        }

        [IntegrationFact]
        public async Task AddEntities()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";
            const int count = 5;
            var entities = new List<Poco>();
            for (int i = 0; i < count; i++)
            {
                entities.Add(new Poco
                {
                    LongValue = longValue,
                    StringValue = stringValue
                });
            }

            // Act
            List<Poco> results = (await _repository.AddAsync(entities)).ToList();

            // Assert
            Assert.Equal(count, entities.Count);
            Assert.True(results.All(p => p.LongValue == longValue));
            Assert.True(results.All(p => p.StringValue == stringValue));
            Assert.True(results.All(p => p.Id != null));
        }

        [IntegrationFact]
        public async Task AddEntitiesWithConflict()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";
            const int count = 5;
            var entities = new List<Poco>();
            for (int i = 0; i < count; i++)
            {
                entities.Add(new Poco
                {
                    LongValue = longValue,
                    StringValue = stringValue
                });
            }

            await _repository.AddAsync(entities);
            Exception exception = null;

            // Act
            try
            {
                await _repository.AddAsync(entities);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ConflictException>(exception);
        }

        [IntegrationFact]
        public async Task UpdateEntity()
        {
            // Arrange
            const long longValue = 555;
            const long newLongValue = 333;
            const string stringValue = "555";
            const string newStringValue = "333";

            Poco entity = await _repository.AddAsync(new Poco
            {
                LongValue = longValue,
                StringValue = stringValue
            });

            entity.LongValue = newLongValue;
            entity.StringValue = newStringValue;

            // Act
            Poco result = await _repository.UpdateAsync(entity);

            // Assert
            Assert.Equal(newLongValue, result.LongValue);
            Assert.Equal(newStringValue, result.StringValue);
            Assert.Equal(entity.Id, result.Id);
        }

        [IntegrationFact]
        public async Task UpdateEntityWithNotFound()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";

            var entity = new Poco
            {
                LongValue = longValue,
                StringValue = stringValue,
                Id = "5"
            };

            Exception exception = null;

            // Act
            try
            {
                await _repository.UpdateAsync(entity);
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
        public async Task UpdateEntityWithoutObjectId()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";

            var entity = new Poco
            {
                LongValue = longValue,
                StringValue = stringValue
            };

            Exception exception = null;

            // Act
            try
            {
                await _repository.UpdateAsync(entity);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [IntegrationFact]
        public async Task UpdateEntities()
        {
            // Arrange
            const long longValue = 555;
            const long newLongValue = 333;
            const string stringValue = "555";
            const string newStringValue = "333";
            const int count = 5;

            var entities = new List<Poco>();
            for (int i = 0; i < count; i++)
            {
                entities.Add(new Poco
                {
                    LongValue = longValue,
                    StringValue = stringValue
                });
            }

            await _repository.AddAsync(entities);
            foreach (Poco entity in entities)
            {
                entity.LongValue = newLongValue;
                entity.StringValue = newStringValue;
            }

            // Act
            List<Poco> results = (await _repository.UpdateAsync(entities)).ToList();

            // Assert
            Assert.Equal(count, entities.Count);
            Assert.True(results.All(p => p.LongValue == newLongValue));
            Assert.True(results.All(p => p.StringValue == newStringValue));
            Assert.True(results.All(p => p.Id != null));
        }

        [IntegrationFact]
        public async Task UpdateEntitiesWithNotFound()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";
            const int count = 5;

            var entities = new List<Poco>();
            for (int i = 0; i < count; i++)
            {
                entities.Add(new Poco
                {
                    LongValue = longValue,
                    StringValue = stringValue,
                    Id = "5"
                });
            }

            // Act
            Exception exception = null;

            // Act
            try
            {
                await _repository.UpdateAsync(entities);
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
        public async Task UpdateEntitiesWithoutObjectId()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";
            const int count = 5;

            var entities = new List<Poco>();
            for (int i = 0; i < count; i++)
            {
                entities.Add(new Poco
                {
                    LongValue = longValue,
                    StringValue = stringValue
                });
            }

            // Act
            Exception exception = null;

            // Act
            try
            {
                await _repository.UpdateAsync(entities);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [IntegrationFact]
        public async Task AddOrUpdateEntityWithInsert()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";

            var entity = new Poco
            {
                LongValue = longValue,
                StringValue = stringValue
            };

            // Act
            Poco result = await _repository.AddOrUpdateAsync(entity);

            // Assert
            Assert.Equal(longValue, result.LongValue);
            Assert.Equal(stringValue, result.StringValue);
            Assert.NotEqual(null, result.Id);
        }

        [IntegrationFact]
        public async Task AddOrUpdateEntityWithUpset()
        {
            // Arrange
            const long longValue = 555;
            const long newLongValue = 333;
            const string stringValue = "555";
            const string newStringValue = "333";

            Poco entity = await _repository.AddAsync(new Poco
            {
                LongValue = longValue,
                StringValue = stringValue
            });

            entity.LongValue = newLongValue;
            entity.StringValue = newStringValue;

            // Act
            Poco result = await _repository.AddOrUpdateAsync(entity);

            // Assert
            Assert.Equal(newLongValue, result.LongValue);
            Assert.Equal(newStringValue, result.StringValue);
            Assert.Equal(entity.Id, result.Id);
        }

        [IntegrationFact]
        public async Task AddOrUpdateEntitiesWithInsert()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";
            const int count = 5;
            var entities = new List<Poco>();
            for (int i = 0; i < count; i++)
            {
                entities.Add(new Poco
                {
                    LongValue = longValue,
                    StringValue = stringValue
                });
            }

            // Act
            List<Poco> results = (await _repository.AddOrUpdateAsync(entities)).ToList();

            // Assert
            Assert.Equal(count, entities.Count);
            Assert.True(results.All(p => p.LongValue == longValue));
            Assert.True(results.All(p => p.StringValue == stringValue));
            Assert.True(results.All(p => p.Id != null));
        }

        [IntegrationFact]
        public async Task AddOrUpdateEntitiesWithUpset()
        {
            // Arrange
            const long longValue = 555;
            const long newLongValue = 333;
            const string stringValue = "555";
            const string newStringValue = "333";

            const int count = 5;
            var entities = new List<Poco>();
            for (int i = 0; i < count; i++)
            {
                entities.Add(new Poco
                {
                    LongValue = longValue,
                    StringValue = stringValue
                });
            }

            await _repository.AddAsync(entities);
            foreach (Poco entity in entities)
            {
                entity.LongValue = newLongValue;
                entity.StringValue = newStringValue;
            }

            // Act
            List<Poco> results = (await _repository.AddOrUpdateAsync(entities)).ToList();

            // Assert
            Assert.Equal(count, entities.Count);
            Assert.True(results.All(p => p.LongValue == newLongValue));
            Assert.True(results.All(p => p.StringValue == newStringValue));
            Assert.True(results.All(p => p.Id != null));
        }

        [IntegrationFact]
        public async Task DeleteEntity()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";

            Poco entity = await _repository.AddAsync(new Poco
            {
                LongValue = longValue,
                StringValue = stringValue
            });

            // Act
            await _repository.DeleteAsync(entity);
        }

        [IntegrationFact]
        public async Task DeleteEntityWithNotFound()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";

            var entity = new Poco
            {
                LongValue = longValue,
                StringValue = stringValue
            };

            Exception exception = null;

            // Act
            try
            {
                await _repository.DeleteAsync(entity);
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
        public async Task DeleteEntities()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";
            const int count = 5;
            var entities = new List<Poco>();
            for (int i = 0; i < count; i++)
            {
                entities.Add(new Poco
                {
                    LongValue = longValue,
                    StringValue = stringValue
                });
            }

            await _repository.AddAsync(entities);

            // Act
            await _repository.DeleteAsync(entities);
        }

        [IntegrationFact]
        public async Task DeleteEntitiesWithNotFound()
        {
            // Arrange
            const long longValue = 555;
            const string stringValue = "555";
            const int count = 5;
            var entities = new List<Poco>();
            for (int i = 0; i < count; i++)
            {
                entities.Add(new Poco
                {
                    LongValue = longValue,
                    StringValue = stringValue
                });
            }

            Exception exception = null;

            // Act
            try
            {
                await _repository.DeleteAsync(entities);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<NotFoundException>(exception);
        }
    }
}