// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using LinkTracker.DAL.Entities;
using LinkTracker.Domain;

namespace LinkTracker.Mappings.Entities
{
    public class TrackingUrlMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<TrackingUrlEntity, DomainTrackingUrl>()
                .ForMember(dest => dest.Key, opt => opt.Ignore())
                .ForMember(dest => dest.RequestUri, opt => opt.Ignore())
                ;

            Mapper.CreateMap<DomainTrackingUrl, TrackingUrlEntity>()
                ;
        }
    }
}