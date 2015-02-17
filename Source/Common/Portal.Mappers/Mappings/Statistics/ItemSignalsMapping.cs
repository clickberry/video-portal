// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using AutoMapper;
using Cassandra;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Statistics;
using Portal.Domain.StatisticContext;

namespace Portal.Mappers.Mappings.Statistics
{
    public class ItemSignalsMapping : IMapping
    {
        private readonly string _dateTimePropertyName = NameOfHelper.PropertyName<ItemSignalsEntity>(x => x.DateTime);
        private readonly string _isAnticolumnPropertyName = NameOfHelper.PropertyName<ItemSignalsEntity>(x => x.IsAnticolumn);
        private readonly string _rowKeyPropertyName = NameOfHelper.PropertyName<ItemSignalsEntity>(x => x.ItemIdSignalType);
        private readonly string _userIdPropertyName = NameOfHelper.PropertyName<ItemSignalsEntity>(x => x.UserId);

        public void Register()
        {
            Mapper.CreateMap<Row, ItemSignalsEntity>()
                .ForMember(d => d.ItemIdSignalType, o => o.MapFrom(row => row.GetValue<string>(_rowKeyPropertyName)))
                .ForMember(d => d.IsAnticolumn, o => o.MapFrom(row => row.GetValue<bool>(_isAnticolumnPropertyName)))
                .ForMember(d => d.UserId, o => o.MapFrom(row => !row.IsNull(_userIdPropertyName) ? row.GetValue<string>(_userIdPropertyName) : null))
                .ForMember(d => d.DateTime, o => o.MapFrom(row => row.GetValue<DateTimeOffset>(_dateTimePropertyName)))
                ;

            Mapper.CreateMap<ItemSignalsEntity, DomainItemSignal>()
                .ForMember(d => d.DateTime, o => o.MapFrom(s => s.DateTime.DateTime))
                ;
        }
    }
}