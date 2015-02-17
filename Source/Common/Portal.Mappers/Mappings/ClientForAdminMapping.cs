// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Admin;

namespace Portal.Mappers.Mappings
{
    public sealed class ClientForAdminMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<SubscriptionEntity, DomainAdminClientSubscription>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => (SubscriptionType)source.Type))
                .ForMember(dest => dest.State, opt => opt.MapFrom(source => (ResourceState)source.State));
            Mapper.CreateMap<AdminClientSubscription, DomainAdminClientSubscription>();
            Mapper.CreateMap<DomainAdminClientSubscription, AdminClientSubscription>();

            Mapper.CreateMap<AdminClient, DomainClientForAdmin>();
            Mapper.CreateMap<DomainClientForAdmin, AdminClient>();
        }
    }
}