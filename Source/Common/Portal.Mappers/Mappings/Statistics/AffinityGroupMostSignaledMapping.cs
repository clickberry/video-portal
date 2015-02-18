// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using AutoMapper;
using Cassandra;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Statistics;
using Portal.Domain.StatisticContext;

namespace Portal.Mappers.Mappings.Statistics
{
    public class AffinityGroupMostSignaledMapping : IMapping
    {
        private readonly string _countPropertyName = NameOfHelper.PropertyName<AffinityGroupMostSignaledEntity>(x => x.Count);
        private readonly string _itemIdPropertyName = NameOfHelper.PropertyName<AffinityGroupMostSignaledEntity>(x => x.ItemId);
        private readonly string _rowKeyPropertyName = NameOfHelper.PropertyName<AffinityGroupMostSignaledEntity>(x => x.AffinityGroupSignalType);

        public void Register()
        {
            Mapper.CreateMap<Row, AffinityGroupMostSignaledEntity>()
                .ForMember(d => d.AffinityGroupSignalType, o => o.MapFrom(row => row.GetValue<string>(_rowKeyPropertyName)))
                .ForMember(d => d.ItemId, o => o.MapFrom(row => row.GetValue<string>(_itemIdPropertyName)))
                .ForMember(d => d.Count, o => o.MapFrom(row => row.GetValue<long>(_countPropertyName)))
                ;

            Mapper.CreateMap<AffinityGroupMostSignaledEntity, DomainMostSignaledItem>()
                .ForMember(d => d.Version, o => o.Ignore())
                ;

            Mapper.CreateMap<Tuple<AffinityGroupMostSignaledEntity, long>, DomainMostSignaledItem>()
                .ConvertUsing(t =>
                {
                    DomainMostSignaledItem result = Mapper.Map<AffinityGroupMostSignaledEntity, DomainMostSignaledItem>(t.Item1);
                    result.Version = t.Item2;
                    return result;
                })
                ;
        }
    }
}