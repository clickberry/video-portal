// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Cassandra;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Statistics;
using Portal.Domain.StatisticContext;

namespace Portal.Mappers.Mappings.Statistics
{
    public class AffinityGroupMostSignaledVersionMapping : IMapping
    {
        private readonly string _rowIdPropertyName = NameOfHelper.PropertyName<AffinityGroupMostSignaledVersionEntity>(x => x.AffinityGroupSignalType);
        private readonly string _versionPropertyName = NameOfHelper.PropertyName<AffinityGroupMostSignaledVersionEntity>(x => x.Version);

        public void Register()
        {
            Mapper.CreateMap<Row, AffinityGroupMostSignaledVersionEntity>()
                .ForMember(d => d.AffinityGroupSignalType, o => o.MapFrom(row => row.GetValue<string>(_rowIdPropertyName)))
                .ForMember(d => d.Version, o => o.MapFrom(row => !row.IsNull(_versionPropertyName) ? row.GetValue<long>(_versionPropertyName) : 0))
                ;

            Mapper.CreateMap<ItemCountsEntity, DomainItemCounts>()
                ;
        }
    }
}