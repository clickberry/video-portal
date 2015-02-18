// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using AutoMapper;
using Cassandra;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Statistics;

namespace Portal.Mappers.Mappings.Statistics
{
    public class TimeSeriesRollupsMinuteMapping : IMapping
    {
        private readonly string _countPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsMinuteEntity>(x => x.Count);
        private readonly string _minutePropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsMinuteEntity>(x => x.Minute);
        private readonly string _rowKeyPropertyName = NameOfHelper.PropertyName<TimeSeriesRollupsMinuteEntity>(x => x.EventIdHh);

        public void Register()
        {
            Mapper.CreateMap<Row, TimeSeriesRollupsMinuteEntity>()
                .ForMember(d => d.EventIdHh, o => o.MapFrom(row => row.GetValue<string>(_rowKeyPropertyName)))
                .ForMember(d => d.Minute, o => o.MapFrom(row => row.GetValue<DateTimeOffset>(_minutePropertyName)))
                .ForMember(d => d.Count, o => o.MapFrom(row => !row.IsNull(_countPropertyName) ? row.GetValue<long>(_countPropertyName) : 0))
                ;
        }
    }
}