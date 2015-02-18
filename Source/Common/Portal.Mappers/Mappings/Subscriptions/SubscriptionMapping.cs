// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Subscriptions;

namespace Portal.Mappers.Mappings.Subscriptions
{
    public class SubscriptionMapping : IMapping
    {
        public void Register()
        {
            // Subscription

            Mapper.CreateMap<SubscriptionEntity, CompanySubscription>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => (SubscriptionType)source.Type))
                ;

            Mapper.CreateMap<CompanySubscription, SubscriptionEntity>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => (int)source.Type))
                .ForMember(dest => dest.State, opt => opt.MapFrom(source => (int)source.State))
                ;


            // Subscription create options

            Mapper.CreateMap<CompanySubscriptionCreateOptions, CompanySubscription>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Created, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore())
                .ForMember(dest => dest.SiteHostname, opt => opt.MapFrom(source => source.SiteHostname.ToLowerInvariant()))
                .ForMember(dest => dest.LastSyncDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastCycleDate, opt => opt.Ignore())
                .ForMember(dest => dest.HasTrialClicks, opt => opt.Ignore())
                ;


            // Subscription update options

            Mapper.CreateMap<CompanySubscription, CompanySubscriptionUpdateOptions>()
                ;


            // Subscription DTO

            Mapper.CreateMap<CompanySubscription, Subscription>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(s => (int)s.State))
                ;

            Mapper.CreateMap<Subscription, CompanySubscription>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(s => (ResourceState)s.State))
                .ForMember(dest => dest.IsManuallyEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LastSyncDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastCycleDate, opt => opt.Ignore())
                .ForMember(dest => dest.HasTrialClicks, opt => opt.Ignore())
                ;
        }
    }
}