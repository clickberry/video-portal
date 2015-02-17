// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoRepository;
using Portal.DAL.Azure.Queries;
using Portal.DAL.Context;
using Portal.Exceptions.CRUD;

namespace Portal.DAL.Azure.Context
{
    public class MongoTableRepository<T> : MongoRepository<T>, ITableRepository<T> where T : class, IEntity
    {
        public MongoTableRepository(MongoUrl url) : base(url)
        {
        }

        public Task<List<T>> ToListAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    return Collection.FindAs<T>(Query.Null).ToList();
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public Task<List<T>> ToListAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.Where(predicate).ToList();
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public Task<List<T>> TakeAsync(int count)
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.Take(count).ToList();
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public Task<List<T>> TakeAsync(Expression<Func<T, bool>> predicate, int count)
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.Where(predicate).Take(count).ToList();
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public Task<T> FirstAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.First();
                }
                catch (InvalidOperationException exception)
                {
                    throw new NotFoundException(exception);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public Task<T> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.First(predicate);
                }
                catch (InvalidOperationException exception)
                {
                    throw new NotFoundException(exception);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public Task<T> FirstOrDefaultAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.FirstOrDefault(predicate);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public Task<T> SingleAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.Single();
                }
                catch (InvalidOperationException exception)
                {
                    throw new NotFoundException(exception);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.Single(predicate);
                }
                catch (InvalidOperationException exception)
                {
                    throw new NotFoundException(exception);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public Task<T> SingleOrDefaultAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.SingleOrDefault();
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.Run(() =>
            {
                try
                {
                    return Context.SingleOrDefault(predicate);
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Mongo driver failure.", e);
                }
            });
        }

        public IEnumerable<T> AsEnumerable()
        {
            return Collection.AsQueryable();
        }

        public IEnumerable<T> AsEnumerable(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return Context.Where(predicate);
            }
            catch (InvalidOperationException exception)
            {
                throw new NotFoundException(exception);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Mongo driver failure.", e);
            }
        }

        public Task DeleteAsync(T entity)
        {
            return Task.Run(() =>
            {
                WriteConcernResult result;

                IMongoQuery query = QueryFactory.Create(entity);

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

        public Task DeleteAsync(IEnumerable<T> entities)
        {
            return Task.WhenAll(entities.Select(DeleteAsync));
        }

        public override Task<T> UpdateAsync(T entity)
        {
            return Task.Run(() =>
            {
                if (entity.Id == ObjectId.Empty.ToString())
                {
                    IMongoQuery mongoQuery = QueryFactory.Create(entity);
                    T oldEntity = Collection.Find(mongoQuery).FirstOrDefault();
                    if (oldEntity == null)
                    {
                        throw new NotFoundException(string.Format("Entity {0} was not found.", typeof (T)));
                    }

                    entity.Id = oldEntity.Id;
                }

                return base.UpdateAsync(entity);
            });
        }
    }
}