// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.SubscriptionContext;

namespace Portal.Mappers.Mappings.Subscriptions
{
    public class CompanyMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<CompanyEntity, DomainCompany>()
                ;

            Mapper.CreateMap<DomainCompany, CompanyEntity>()
                .ForMember(d => d.NameSort, o => o.MapFrom(s => s.Name != null ? s.Name.ToLowerInvariant() : null))
                ;

            Mapper.CreateMap<DomainPendingClient, PendingClientEntity>()
                ;

            Mapper.CreateMap<PendingClientEntity, DomainPendingClient>()
                ;
        }
    }
}