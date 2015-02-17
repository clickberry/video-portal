// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Portal.DTO.Trends;
using Portal.DTO.Watch;

namespace Portal.Mappers.Mappings
{
    public sealed class WatchMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<Watch, TrendingWatch>()
                .ForMember(d => d.Version, o => o.Ignore())
                ;

            Mapper.CreateMap<TrendingWatch, Watch>()
                ;
        }
    }
}