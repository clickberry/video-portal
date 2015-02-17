// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using AutoMapper;
using Cassandra;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Statistics;

namespace Portal.Mappers.Mappings.Statistics
{
    public class TimeSeriesRawMapping : IMapping
    {
        private readonly string _dateTimePropertyName = NameOfHelper.PropertyName<TimeSeriesRawEntity>(x => x.DateTime);
        private readonly string _rowKeyPropertyName = NameOfHelper.PropertyName<TimeSeriesRawEntity>(x => x.EventIdYymmddhh);
        private readonly string _payloadPropertyName = NameOfHelper.PropertyName<TimeSeriesRawEntity>(x => x.Payload);

        public void Register()
        {
            Mapper.CreateMap<Row, TimeSeriesRawEntity>()
                .ForMember(d => d.EventIdYymmddhh, o => o.MapFrom(row => row.GetValue<string>(_rowKeyPropertyName)))
                .ForMember(d => d.DateTime, o => o.MapFrom(row => row.GetValue<DateTimeOffset>(_dateTimePropertyName)))
                .ForMember(d => d.Payload, o => o.MapFrom(row => !row.IsNull(_payloadPropertyName) ? row.GetValue<string>(_payloadPropertyName) : null))
                ;
        }
    }
}