// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using AutoMapper;
using Cassandra;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Statistics;

namespace Portal.Mappers.Mappings.Statistics
{
    public class UserSignalsUnorderedMapping : IMapping
    {
        private readonly string _dateTimePropertyName = NameOfHelper.PropertyName<UserSignalsUnorderedEntity>(x => x.DateTime);
        private readonly string _isAnticolumnPropertyName = NameOfHelper.PropertyName<UserSignalsUnorderedEntity>(x => x.IsAnticolumn);
        private readonly string _itemIdPropertyName = NameOfHelper.PropertyName<UserSignalsUnorderedEntity>(x => x.ItemId);
        private readonly string _rowKeyPropertyName = NameOfHelper.PropertyName<UserSignalsUnorderedEntity>(x => x.UserIdSignalType);

        public void Register()
        {
            Mapper.CreateMap<Row, UserSignalsUnorderedEntity>()
                .ForMember(d => d.UserIdSignalType, o => o.MapFrom(row => row.GetValue<string>(_rowKeyPropertyName)))
                .ForMember(d => d.IsAnticolumn, o => o.MapFrom(row => row.GetValue<bool>(_isAnticolumnPropertyName)))
                .ForMember(d => d.ItemId, o => o.MapFrom(row => row.GetValue<string>(_itemIdPropertyName)))
                .ForMember(d => d.DateTime, o => o.MapFrom(row => row.GetValue<DateTimeOffset>(_dateTimePropertyName)))
                ;
        }
    }
}