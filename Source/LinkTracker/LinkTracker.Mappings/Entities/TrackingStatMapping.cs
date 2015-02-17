// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using LinkTracker.Domain;
using Portal.Domain.SubscriptionContext;

namespace LinkTracker.Mappings.Entities
{
    public class TrackingStatMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<UrlTrackingResult, DomainTrackingStat>()
                .ForMember(dest => dest.RedirectUrl, opt => opt.MapFrom(source => source.Redirect.AbsoluteUri))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                ;
        }
    }
}