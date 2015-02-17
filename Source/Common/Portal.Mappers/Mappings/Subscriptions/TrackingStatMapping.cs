// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Subscriptions;

namespace Portal.Mappers.Mappings.Subscriptions
{
    public class TrackingStatMapping : IMapping
    {
        public void Register()
        {
            // Stat
            Mapper.CreateMap<TrackingStatEntity, DomainTrackingStat>()
                ;

            Mapper.CreateMap<DomainTrackingStat, TrackingStatEntity>()
                ;


            // Aggregated stat
            Mapper.CreateMap<TrackingStatPerUrlEntity, DomainTrackingStatPerUrl>()
                ;

            Mapper.CreateMap<TrackingStatPerDateEntity, DomainTrackingStatPerDate>()
                ;


            // Dto
            Mapper.CreateMap<DomainTrackingStat, TrackingStat>()
                ;

            Mapper.CreateMap<DomainTrackingStatPerUrl, TrackingStatPerUrl>()
                ;

            Mapper.CreateMap<DomainTrackingStatPerDate, TrackingStatPerDate>()
                ;
        }
    }
}