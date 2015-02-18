// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using AutoMapper;
using Cassandra;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Statistics;

namespace Portal.Mappers.Mappings.Statistics
{
    public class AffinityGroupItemCountsMapping : IMapping
    {
        private readonly string _countPropertyName = NameOfHelper.PropertyName<AffinityGroupItemCountsEntity>(x => x.Count);
        private readonly string _itemIdPropertyName = NameOfHelper.PropertyName<AffinityGroupItemCountsEntity>(x => x.ItemId);
        private readonly string _rowIdPropertyName = NameOfHelper.PropertyName<AffinityGroupItemCountsEntity>(x => x.AffinityGroupSignalType);

        public void Register()
        {
            Mapper.CreateMap<Row, AffinityGroupItemCountsEntity>()
                .ForMember(d => d.AffinityGroupSignalType, o => o.MapFrom(row => row.GetValue<string>(_rowIdPropertyName)))
                .ForMember(d => d.ItemId, o => o.MapFrom(row => row.GetValue<string>(_itemIdPropertyName)))
                .ForMember(d => d.Count, o => o.MapFrom(row => !row.IsNull(_countPropertyName) ? row.GetValue<long>(_countPropertyName) : 0))
                ;
        }
    }
}