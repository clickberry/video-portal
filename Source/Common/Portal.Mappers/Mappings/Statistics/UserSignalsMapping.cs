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
    public class UserSignalsMapping : IMapping
    {
        private readonly string _dateTimePropertyName = NameOfHelper.PropertyName<UserSignalsEntity>(x => x.DateTime);
        private readonly string _isAnticolumnPropertyName = NameOfHelper.PropertyName<UserSignalsEntity>(x => x.IsAnticolumn);
        private readonly string _itemIdPropertyName = NameOfHelper.PropertyName<UserSignalsEntity>(x => x.ItemId);
        private readonly string _rowKeyPropertyName = NameOfHelper.PropertyName<UserSignalsEntity>(x => x.UserIdSignalType);

        public void Register()
        {
            Mapper.CreateMap<Row, UserSignalsEntity>()
                .ForMember(d => d.UserIdSignalType, o => o.MapFrom(row => row.GetValue<string>(_rowKeyPropertyName)))
                .ForMember(d => d.IsAnticolumn, o => o.MapFrom(row => row.GetValue<bool>(_isAnticolumnPropertyName)))
                .ForMember(d => d.ItemId, o => o.MapFrom(row => row.GetValue<string>(_itemIdPropertyName)))
                .ForMember(d => d.DateTime, o => o.MapFrom(row => row.GetValue<DateTimeOffset>(_dateTimePropertyName)))
                ;

            Mapper.CreateMap<UserSignalsEntity, DomainUserSignal>()
                .ForMember(d => d.DateTime, o => o.MapFrom(s => s.DateTime.DateTime))
                ;
        }
    }
}