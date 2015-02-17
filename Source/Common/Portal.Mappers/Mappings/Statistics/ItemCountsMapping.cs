// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Cassandra;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Statistics;
using Portal.Domain.StatisticContext;

namespace Portal.Mappers.Mappings.Statistics
{
    public class ItemCountsMapping : IMapping
    {
        private readonly string _itemIdPropertyName = NameOfHelper.PropertyName<ItemCountsEntity>(x => x.ItemId);
        private readonly string _likesPropertyName = NameOfHelper.PropertyName<ItemCountsEntity>(x => x.Likes);
        private readonly string _dislikesPropertyName = NameOfHelper.PropertyName<ItemCountsEntity>(x => x.Dislikes);
        private readonly string _viewsPropertyName = NameOfHelper.PropertyName<ItemCountsEntity>(x => x.Views);
        private readonly string _abusesPropertyName = NameOfHelper.PropertyName<ItemCountsEntity>(x => x.Abuses);

        public void Register()
        {
            Mapper.CreateMap<Row, ItemCountsEntity>()
                .ForMember(d => d.ItemId, o => o.MapFrom(row => row.GetValue<string>(_itemIdPropertyName)))
                .ForMember(d => d.Views, o => o.MapFrom(row => !row.IsNull(_viewsPropertyName) ? row.GetValue<long>(_viewsPropertyName) : 0))
                .ForMember(d => d.Likes, o => o.MapFrom(row => !row.IsNull(_likesPropertyName) ? row.GetValue<long>(_likesPropertyName) : 0))
                .ForMember(d => d.Dislikes, o => o.MapFrom(row => !row.IsNull(_dislikesPropertyName) ? row.GetValue<long>(_dislikesPropertyName) : 0))
                .ForMember(d => d.Abuses, o => o.MapFrom(row => !row.IsNull(_abusesPropertyName) ? row.GetValue<long>(_abusesPropertyName) : 0))
                ;

            Mapper.CreateMap<ItemCountsEntity, DomainItemCounts>()
                ;
        }
    }
}