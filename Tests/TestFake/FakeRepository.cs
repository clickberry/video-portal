using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Portal.DAL.Context;
using Portal.DAL.Entities.QueryObject;
using Portal.DAL.Entities.Table;

namespace TestFake
{
    public class FakeRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly List<T> _entities;

        public FakeRepository(List<T> entities)
        {
            _entities = entities;
        }


        public Task<T> AddAsync(T entity, CancellationToken cancellationToken = new CancellationToken())
        {
            _entities.Add(entity);
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(entity);

            return tcs.Task;
        }

        public Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = new CancellationToken())
        {
            _entities.AddRange(entities);
            var tcs = new TaskCompletionSource<IEnumerable<T>>();
            tcs.SetResult(entities);

            return tcs.Task;
        }

        public Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entity, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> AddOrUpdateAsync(T entity, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> AddOrUpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = new CancellationToken())
        {
            _entities.AddRange(entities);
            var tcs = new TaskCompletionSource<IEnumerable<T>>();
            tcs.SetResult(entities);

            return tcs.Task;
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IEnumerable<T> entity, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> ToListAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var tcs = new TaskCompletionSource<List<T>>();
            var result = _entities.ToList();
            tcs.SetResult(result);

            return tcs.Task;
        }

        public Task<List<T>> ToListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new CancellationToken())
        {
            var tcs = new TaskCompletionSource<List<T>>();
            var result = _entities.Where(predicate.Compile()).ToList();
            tcs.SetResult(result);

            return tcs.Task;
        }

        public Task<List<T>> TakeAsync(int count, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> TakeAsync(Expression<Func<T, bool>> predicate, int count, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new CancellationToken())
        {
            var tcs = new TaskCompletionSource<T>();
            var result = _entities.FirstOrDefault(predicate.Compile());
            tcs.SetResult(result);

            return tcs.Task;
        }

        public Task<T> SingleAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> SingleOrDefaultAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> AsEnumerable()
        {
            return _entities;   
        }

        public IEnumerable<T> AsEnumerable(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate.Compile());
        }

        public IQueryable<T> AsQueryable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetStatEntities(StatQueryObject queryObject)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetReportEntities(ReportQueryObject queryObject)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetLastReport(ReportQueryObject queryObject)
        {
            throw new NotImplementedException();
        }

        public List<T> GetHitsCounts(HitsCountQueryObject queryObject)
        {
            throw new NotImplementedException();
        }

        public T GetLastHitsCountUpdate(HitsCountQueryObject queryObject)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _entities.FirstOrDefault(predicate.Compile());
        }
    }
}