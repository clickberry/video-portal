// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Portal.Exceptions.CRUD;

namespace MongoRepository
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly MongoCollection<T> _collection;

        public MongoRepository(MongoUrl url)
        {
            var client = new MongoClient(url);

            MongoServer server = client.GetServer();
            MongoDatabase database = server.GetDatabase(url.DatabaseName);

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            Attribute attribute = Attribute.GetCustomAttribute(typeof (T), typeof (CollectionNameAttribute));
            string collection = attribute != null ? ((CollectionNameAttribute)attribute).Name : typeof (T).Name;

            _collection = database.GetCollection<T>(collection);
        }

        public MongoCollection<T> Collection
        {
            get { return _collection; }
        }

        public Task<T> AddAsync(T entity)
        {
            return Task.Run(() =>
            {
                WriteConcernResult result;

                try
                {
                    result = Collection.Insert(entity);
                }
                catch (MongoWriteConcernException e)
                {
                    throw HandleWriteException(e);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }

                CheckWriteResult(result);

                return entity;
            });
        }

        public Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities)
        {
            return Task.Run(() =>
            {
                IEnumerable<WriteConcernResult> results;

                List<T> args = entities.ToList();

                try
                {
                    results = Collection.InsertBatch(args);
                }
                catch (MongoWriteConcernException e)
                {
                    throw HandleWriteException(e);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }

                foreach (WriteConcernResult result in results)
                {
                    CheckWriteResult(result);
                }

                return args.AsEnumerable();
            });
        }

        public Task<T> AddOrUpdateAsync(T entity)
        {
            return Task.Run(() =>
            {
                WriteConcernResult result;

                try
                {
                    result = Collection.Save(entity);
                }
                catch (MongoWriteConcernException e)
                {
                    throw HandleWriteException(e);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }

                CheckWriteResult(result);

                return entity;
            });
        }

        public async Task<IEnumerable<T>> AddOrUpdateAsync(IEnumerable<T> entities)
        {
            return await Task.WhenAll(entities.Select(AddOrUpdateAsync));
        }

        public Task<List<T>> GetAllAsync()
        {
            return Task.FromResult(Collection.FindAll().ToList());
        }

        public virtual Task<T> UpdateAsync(T entity)
        {
            return Task.Run(() =>
            {
                WriteConcernResult result;

                IMongoQuery query = Query<T>.EQ(arg => arg.Id, entity.Id);
                IMongoUpdate update = Update.Replace(entity);

                try
                {
                    result = Collection.Update(query, update);
                }
                catch (MongoWriteConcernException e)
                {
                    throw HandleWriteException(e);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }

                CheckWriteResult(result, true);

                return entity;
            });
        }

        public Task<T> GetAsync(string id)
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.FirstOrDefault(i => i.Id == id);
                }
                catch (FormatException)
                {
                    throw new NotFoundException("Entity was not found.");
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities)
        {
            return await Task.WhenAll(entities.Select(UpdateAsync));
        }

        public Task DeleteAsync(string id)
        {
            return Task.Run(() =>
            {
                WriteConcernResult result;

                IMongoQuery query = Query<T>.EQ(q => q.Id, id);

                try
                {
                    result = Collection.Remove(query);
                }
                catch (MongoWriteConcernException e)
                {
                    throw HandleWriteException(e);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }

                CheckWriteResult(result, true);
            });
        }

        public IQueryable<T> Context
        {
            get { return _collection.AsQueryable(); }
        }


        protected static Exception HandleWriteException(MongoWriteConcernException exception)
        {
            switch (exception.Code)
            {
                case 11000:
                case 11001:
                    throw new ConflictException(exception.ErrorMessage, exception);
            }

            return new InternalServerErrorException(exception);
        }

        protected static void CheckWriteResult(WriteConcernResult result, bool checkModification = false)
        {
            if (!result.Ok)
            {
                throw new BadRequestException(string.Format("Bad request: {0}", result.LastErrorMessage));
            }

            if (!checkModification)
            {
                return;
            }

            if (result.DocumentsAffected == 0)
            {
                throw new NotFoundException("Entity was not found.");
            }

            if (result.DocumentsAffected > 1)
            {
                throw new InvalidOperationException("Was affected multiple documents.");
            }
        }

        protected static Exception HandleExceptions(Exception exception)
        {
            if (exception is FileFormatException)
            {
                throw new EntityTooLargeException();
            }

            return new InternalServerErrorException(exception);
        }
    }
}