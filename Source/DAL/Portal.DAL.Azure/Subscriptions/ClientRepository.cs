using System;
using System.Linq;
using System.Threading.Tasks;
using MongoRepository;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;

namespace Portal.DAL.Azure.Subscriptions
{
    public class ClientRepository : IClientRepository
    {
        private readonly MongoRepository<ClientEntity> _collection;

        public ClientRepository(string connectionString)
        {
            _collection = new MongoRepository<ClientEntity>(connectionString);
        }


        public IQueryable<ClientEntity> AsQueryable()
        {
            return _collection;
        }

        public async Task<ClientEntity> GetAsync(ClientEntity entity)
        {
            if (!string.IsNullOrEmpty(entity.Id))
            {
                return _collection.FirstOrDefault(s => s.Id == entity.Id);
            }

            if (!string.IsNullOrEmpty(entity.UserId))
            {
                return _collection.FirstOrDefault(s => s.UserId == entity.UserId);
            }

            return null;
        }

        public async Task<ClientEntity> AddAsync(ClientEntity entity)
        {
            ClientEntity result;
            try
            {
                result = _collection.Add(entity);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Mongo driver failure: {0}", e));
            }

            return result;
        }

        public async Task<ClientEntity> EditAsync(ClientEntity entity)
        {
            ClientEntity result;
            try
            {
                result = _collection.Update(entity);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Mongo driver failure: {0}", e));
            }

            return result;
        }

        public async Task DeleteAsync(ClientEntity entity)
        {
            try
            {
                _collection.Delete(entity);
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Mongo driver failure: {0}", e));
            }
        }
    }
}