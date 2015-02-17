// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Portal.Domain.BillingContext;
using Stripe;

namespace Portal.Mappers.Mappings.Billing
{
    public class CardMapping : IMapping
    {
        public void Register()
        {
            // Card

            Mapper.CreateMap<StripeCard, DomainCard>()
                ;

            Mapper.CreateMap<DomainCard, StripeCard>()
                .ForMember(d => d.AddressCity, o => o.Ignore())
                .ForMember(d => d.AddressCountry, o => o.Ignore())
                .ForMember(d => d.AddressLine1, o => o.Ignore())
                .ForMember(d => d.AddressLine1Check, o => o.Ignore())
                .ForMember(d => d.AddressLine2, o => o.Ignore())
                .ForMember(d => d.AddressState, o => o.Ignore())
                .ForMember(d => d.AddressZip, o => o.Ignore())
                .ForMember(d => d.AddressZipCheck, o => o.Ignore())
                .ForMember(d => d.Country, o => o.Ignore())
                .ForMember(d => d.CvcCheck, o => o.Ignore())
                .ForMember(d => d.ExpirationMonth, o => o.Ignore())
                .ForMember(d => d.ExpirationYear, o => o.Ignore())
                .ForMember(d => d.Fingerprint, o => o.Ignore())
                .ForMember(d => d.Last4, o => o.Ignore())
                .ForMember(d => d.Name, o => o.Ignore())
                .ForMember(d => d.Type, o => o.Ignore())
                ;


            // Card create options

            Mapper.CreateMap<StripeCardCreateOptions, DomainCardCreateOptions>()
                .ForMember(d => d.CustomerId, o => o.Ignore())
                ;

            Mapper.CreateMap<DomainCardCreateOptions, StripeCardCreateOptions>()
                .ForMember(d => d.CardNumber, o => o.Ignore())
                .ForMember(d => d.CardExpirationMonth, o => o.Ignore())
                .ForMember(d => d.CardExpirationYear, o => o.Ignore())
                .ForMember(d => d.CardCvc, o => o.Ignore())
                .ForMember(d => d.CardName, o => o.Ignore())
                .ForMember(d => d.CardAddressLine1, o => o.Ignore())
                .ForMember(d => d.CardAddressLine2, o => o.Ignore())
                .ForMember(d => d.CardAddressZip, o => o.Ignore())
                .ForMember(d => d.CardAddressCity, o => o.Ignore())
                .ForMember(d => d.CardAddressState, o => o.Ignore())
                .ForMember(d => d.CardAddressCountry, o => o.Ignore())
                ;
        }
    }
}