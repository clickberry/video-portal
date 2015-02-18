// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using AutoMapper;
using Portal.Domain.BillingContext;
using Stripe;

namespace Portal.Mappers.Mappings.Billing
{
    public class CustomerMapping : IMapping
    {
        public void Register()
        {
            // Customer

            Mapper.CreateMap<StripeCustomer, DomainCustomer>()
                .ForMember(dest => dest.IsDeleted,
                    opt => opt.MapFrom(source => source.Deleted.HasValue && source.Deleted.Value))
                .ForMember(dest => dest.AccountBalanceInCents,
                    opt => opt.MapFrom(source => source.AccountBalance.HasValue ? source.AccountBalance.Value : 0))
                ;

            Mapper.CreateMap<DomainCustomer, StripeCustomer>()
                .ForMember(dest => dest.Deleted, opt => opt.MapFrom(source => source.IsDeleted))
                .ForMember(dest => dest.AccountBalance, opt => opt.MapFrom(source => source.AccountBalanceInCents))
                .ForMember(dest => dest.Delinquent, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(dest => dest.LiveMode, opt => opt.Ignore())
                .ForMember(dest => dest.Metadata, opt => opt.Ignore())
                .ForMember(dest => dest.StripeCardList, opt => opt.Ignore())
                .ForMember(dest => dest.StripeDefaultCardId, opt => opt.Ignore())
                .ForMember(dest => dest.StripeDiscount, opt => opt.Ignore())
                .ForMember(dest => dest.StripeSubscriptionList, opt => opt.Ignore())
                ;


            // Customer create options

            Mapper.CreateMap<StripeCustomerCreateOptions, DomainCustomerCreateOptions>()
                ;

            Mapper.CreateMap<DomainCustomerCreateOptions, StripeCustomerCreateOptions>()
                .ForMember(source => source.AccountBalance, opt => opt.Ignore())
                .ForMember(source => source.Description, opt => opt.Ignore())
                .ForMember(source => source.Metadata, opt => opt.Ignore())
                .ForMember(source => source.Quantity, opt => opt.Ignore())
                .ForMember(source => source.TrialEnd, opt => opt.Ignore())
                .ForMember(source => source.CouponId, opt => opt.Ignore())
                .ForMember(source => source.CardNumber, opt => opt.Ignore())
                .ForMember(source => source.CardExpirationMonth, opt => opt.Ignore())
                .ForMember(source => source.CardExpirationYear, opt => opt.Ignore())
                .ForMember(source => source.CardCvc, opt => opt.Ignore())
                .ForMember(source => source.CardName, opt => opt.Ignore())
                .ForMember(source => source.CardAddressLine1, opt => opt.Ignore())
                .ForMember(source => source.CardAddressLine2, opt => opt.Ignore())
                .ForMember(source => source.CardAddressZip, opt => opt.Ignore())
                .ForMember(source => source.CardAddressCity, opt => opt.Ignore())
                .ForMember(source => source.CardAddressState, opt => opt.Ignore())
                .ForMember(source => source.CardAddressCountry, opt => opt.Ignore())
                .ForMember(source => source.PlanId, opt => opt.Ignore())
                .ForMember(source => source.TokenId, opt => opt.Ignore())
                ;
        }
    }
}