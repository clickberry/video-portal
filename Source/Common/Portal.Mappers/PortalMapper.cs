// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Portal.Mappers.Mappings;
using Portal.Mappers.Mappings.Billing;
using Portal.Mappers.Mappings.Statistics;
using Portal.Mappers.Mappings.Subscriptions;

namespace Portal.Mappers
{
    public sealed class PortalMapper : IMapper
    {
        private Dictionary<Type, IMapping> _mappings;

        public PortalMapper()
        {
            InitializeCore();
            InitializeMappings
                (
                    new VideoQueueMapping(),
                    new ProcessedVideoMapping(),
                    new ProcessedScreenshotMapping(),
                    new ProjectMapping(),
                    new UserMapping(),
                    new DataQueryOptionsMapping(),
                    new ProjectVideoMapping(),
                    new ProjectAvsxMapping(),
                    new ProjectScreenshotMapping(),
                    new ExternalVideoMapping(),
                    new SendEmailMapping(),
                    new PushNotificationMappings(),
                    new UserForAdminMapping(),
                    new ProjectForAdminMapping(),
                    new ClientForAdminMapping(),
                    new ClientMapping(),
                    new SubscriptionMapping(),
                    new TrackingStatMapping(),
                    new BalanceHistoryMapping(),
                    new CustomerMapping(),
                    new ChargeMapping(),
                    new CardMapping(),
                    new EventMapping(),
                    new CompanyMapping(),
                    new CommentMapping(),
                    new ItemCountsMapping(),
                    new UserCountsMapping(),
                    new ItemSignalsMapping(),
                    new UserSignalsMapping(),
                    new UserSignalsUnorderedMapping(),
                    new AffinityGroupCountsMapping(),
                    new AffinityGroupMostSignaledMapping(),
                    new AffinityGroupMostSignaledVersionMapping(),
                    new AffinityGroupItemCountsMapping(),
                    new TimeSeriesRawMapping(),
                    new TimeSeriesRollupsDayMapping(),
                    new TimeSeriesRollupsHourMapping(),
                    new TimeSeriesRollupsMinuteMapping(),
                    new WatchMapping()
                );

            Mapper.AssertConfigurationIsValid();
        }

        public TOut Map<TIn, TOut>(TIn source)
        {
            return Mapper.Map<TIn, TOut>(source);
        }

        public IQueryable<TOut> Project<TIn, TOut>(IQueryable<TIn> source)
        {
            return source.Project().To<TOut>();
        }

        private void InitializeCore()
        {
            Mapper.CreateMap<string, bool>().ConvertUsing(Convert.ToBoolean);
            Mapper.CreateMap<bool, string>().ConvertUsing(Convert.ToString);

            Mapper.CreateMap<string, double>().ConvertUsing(p => double.Parse(p, CultureInfo.InvariantCulture));
            Mapper.CreateMap<double, string>().ConvertUsing(p => p.ToString(CultureInfo.InvariantCulture));

            Mapper.CreateMap<double, long>().ConvertUsing(Convert.ToInt64);
            Mapper.CreateMap<long, double>().ConvertUsing(Convert.ToDouble);
        }

        private void InitializeMappings(params IMapping[] mappings)
        {
            if (mappings == null)
            {
                throw new ArgumentNullException("mappings");
            }

            _mappings = new Dictionary<Type, IMapping>();

            foreach (IMapping mapping in mappings)
            {
                _mappings.Add(mapping.GetType(), mapping);
                mapping.Register();
            }
        }
    }
}