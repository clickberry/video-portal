// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using AutoMapper;
using Cassandra;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Statistics;

namespace Portal.Mappers.Mappings.Statistics
{
    public class TimeSeriesRollupsDayMapping : IMapping
    {
        private readonly string _countPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsDayEntity>(x => x.Count);
        private readonly string _dayPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsDayEntity>(x => x.Day);
        private readonly string _rowKeyPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsDayEntity>(x => x.EventId);

        public void Register()
        {
            Mapper.CreateMap<Row, TimeSeriesRollupsDayEntity>()
                .ForMember(d => d.EventId, o => o.MapFrom(row => row.GetValue<string>(_rowKeyPropertyName)))
                .ForMember(d => d.Day, o => o.MapFrom(row => row.GetValue<DateTimeOffset>(_dayPropertyName)))
                .ForMember(d => d.Count, o => o.MapFrom(row => !row.IsNull(_countPropertyName) ? row.GetValue<long>(_countPropertyName) : 0))
                ;
        }
    }
}