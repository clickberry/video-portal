// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Subscriptions;

namespace Portal.Mappers.Mappings.Subscriptions
{
    public class BalanceHistoryMapping : IMapping
    {
        public void Register()
        {
            // BalanceHistory
            Mapper.CreateMap<BalanceHistoryEntity, DomainBalanceHistory>()
                ;

            Mapper.CreateMap<DomainBalanceHistory, BalanceHistoryEntity>()
                ;

            // Dto
            Mapper.CreateMap<DomainBalanceHistory, BalanceHistory>()
                ;
        }
    }
}