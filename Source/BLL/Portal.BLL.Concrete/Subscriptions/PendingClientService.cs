// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using MongoRepository;
using Portal.BLL.Infrastructure;
using Portal.BLL.Subscriptions;
using Portal.DAL.Entities.Table;
using Portal.Domain.SubscriptionContext;
using Portal.Exceptions.CRUD;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Subscriptions
{
    public class PendingClientService : IPendingClientService
    {
        private readonly ICryptoService _cryptoService;
        private readonly IMapper _mapper;
        private readonly IRepository<PendingClientEntity> _pendingClientRepository;
        private readonly IStringEncryptor _stringEncryptor;

        public PendingClientService(
            IRepository<PendingClientEntity> pendingClientRepository,
            IStringEncryptor stringEncryptor,
            ICryptoService cryptoService,
            IMapper mapper)
        {
            _pendingClientRepository = pendingClientRepository;
            _stringEncryptor = stringEncryptor;
            _cryptoService = cryptoService;
            _mapper = mapper;
        }


        public async Task<DomainPendingClient> AddAsync(DomainPendingClient client)
        {
            PendingClientEntity entity = _mapper.Map<DomainPendingClient, PendingClientEntity>(client);

            entity.Created = DateTime.UtcNow;
            entity.PasswordSalt = _cryptoService.GenerateSalt();
            entity.Password = _cryptoService.EncodePassword(entity.Password, entity.PasswordSalt);

            entity = await _pendingClientRepository.AddAsync(entity);
            entity.Id = _stringEncryptor.EncryptString(entity.Id);

            return _mapper.Map<PendingClientEntity, DomainPendingClient>(entity);
        }

        public async Task<DomainPendingClient> GetAndDeleteAsync(string id)
        {
            PendingClientEntity entity = await _pendingClientRepository.GetAsync(_stringEncryptor.DecryptString(id));
            if (entity == null)
            {
                throw new NotFoundException();
            }

            await _pendingClientRepository.DeleteAsync(entity.Id);

            return _mapper.Map<PendingClientEntity, DomainPendingClient>(entity);
        }
    }
}