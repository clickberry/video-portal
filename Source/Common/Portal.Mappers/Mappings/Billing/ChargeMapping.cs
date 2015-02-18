// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using AutoMapper;
using Portal.Domain.BillingContext;
using Stripe;

namespace Portal.Mappers.Mappings.Billing
{
    public class ChargeMapping : IMapping
    {
        public void Register()
        {
            // Charge

            Mapper.CreateMap<StripeCharge, DomainCharge>()
                .ForMember(d => d.AmountInCents,
                    o => o.MapFrom(s => s.Amount.HasValue ? s.Amount.Value : 0))
                .ForMember(d => d.IsPaid, o => o.MapFrom(s => s.Paid.HasValue && s.Paid.Value))
                ;

            Mapper.CreateMap<DomainCharge, StripeCharge>()
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.AmountInCents))
                .ForMember(d => d.Paid, o => o.MapFrom(s => s.IsPaid))
                .ForMember(d => d.AmountRefunded, o => o.Ignore())
                .ForMember(d => d.Captured, o => o.Ignore())
                .ForMember(d => d.FailureCode, o => o.Ignore())
                .ForMember(d => d.FailureMessage, o => o.Ignore())
                .ForMember(d => d.LiveMode, o => o.Ignore())
                .ForMember(d => d.Metadata, o => o.Ignore())
                .ForMember(d => d.Refunded, o => o.Ignore())
                .ForMember(d => d.StripeCard, o => o.Ignore())
                ;


            // Charge create options

            Mapper.CreateMap<StripeChargeCreateOptions, DomainChargeCreateOptions>()
                .ForMember(d => d.AmountInCents,
                    o => o.MapFrom(s => s.Amount.HasValue ? s.Amount.Value : 0))
                ;

            Mapper.CreateMap<DomainChargeCreateOptions, StripeChargeCreateOptions>()
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.AmountInCents))
                .ForMember(d => d.ApplicationFee, o => o.Ignore())
                .ForMember(d => d.Capture, o => o.Ignore())
                .ForMember(d => d.Metadata, o => o.Ignore())
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
                .ForMember(d => d.TokenId, o => o.Ignore())
                ;
        }
    }
}