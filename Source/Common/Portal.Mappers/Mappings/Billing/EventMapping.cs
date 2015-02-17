// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.BillingContext;
using Portal.Mappers.ValueResolvers;
using Stripe;

namespace Portal.Mappers.Mappings.Billing
{
    public class EventMapping : IMapping
    {
        public void Register()
        {
            // Stripe Event

            Mapper.CreateMap<StripeEvent, DomainEvent>()
                .ForMember(d => d.LiveMode, o => o.MapFrom(s => s.LiveMode.HasValue && s.LiveMode.Value))
                .ForMember(d => d.Object, o => o.MapFrom(s => s.Data.Object))
                .ForMember(d => d.Type, o => o.ResolveUsing<StripeEventToEventTypeResolver>())
                .ForMember(d => d.ObjectId, o => o.ResolveUsing<StripeEventToIdResolver>());

            // Event entity
            Mapper.CreateMap<DomainEvent, BillingEventEntity>();

            Mapper.CreateMap<BillingEventEntity, DomainEvent>()
                .ForMember(d => d.Object, o => o.Ignore());
        }
    }
}