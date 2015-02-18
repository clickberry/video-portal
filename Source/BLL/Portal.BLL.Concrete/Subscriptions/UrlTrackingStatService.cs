// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Subscriptions;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;
using Portal.Domain;
using Portal.Domain.Admin;
using Portal.Domain.SubscriptionContext;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Subscriptions
{
    public class UrlTrackingStatService : IUrlTrackingStatService
    {
        private readonly IMapper _mapper;
        private readonly ITrackingStatRepository _trackingStatRepository;

        public UrlTrackingStatService(ITrackingStatRepository trackingStatRepository,
            IMapper mapper)
        {
            _trackingStatRepository = trackingStatRepository;
            _mapper = mapper;
        }


        public async Task CountAsync(DomainTrackingStat trackingUrl)
        {
            TrackingStatEntity entity = _mapper.Map<DomainTrackingStat, TrackingStatEntity>(trackingUrl);
            entity.Date = DateTime.UtcNow;

            // Counting url redirect
            await _trackingStatRepository.AddAsync(entity);
        }

        public async Task<long> GetTotalAsync(string subscriptionId, DateTime? @from, DateTime? to)
        {
            IQueryable<TrackingStatEntity> stat = _trackingStatRepository.Context;

            if (from.HasValue)
            {
                stat = stat.Where(s => s.Date >= from.Value);
            }

            if (to.HasValue)
            {
                stat = stat.Where(s => s.Date < to.Value);
            }

            if (!string.IsNullOrEmpty(subscriptionId))
            {
                stat = stat.Where(s => s.SubscriptionId == subscriptionId);
            }

            long total = stat.LongCount();

            return total;
        }

        public async Task<DataResult<DomainTrackingStatPerUrl>> GetStatsPerUrlAsync(string subscriptionId, DataQueryOptions filter)
        {
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }

            // Filter
            string redirectUrlFilter = null;
            DateTime? dateFromFilter = null;
            DateTime? dateToFilter = null;
            List<DataFilterRule> filters = filter.Filters.Where(f => f.Value != null).ToList();
            foreach (DataFilterRule dataFilterRule in filters)
            {
                DataFilterRule f = dataFilterRule;

                if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainTrackingStatPerUrl>(x => x.RedirectUrl),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by redirect url
                    redirectUrlFilter = f.Value.ToString();
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainTrackingStatPerUrl>(x => x.DateFrom),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by date from
                    var date = (DateTime)f.Value;
                    dateFromFilter = date;
                }
                else if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainTrackingStatPerUrl>(x => x.DateTo),
                    StringComparison.OrdinalIgnoreCase) == 0 && f.Type == DataFilterTypes.Equal)
                {
                    // by date to
                    var date = (DateTime)f.Value;
                    dateToFilter = date.AddDays(1);
                }
                else
                {
                    throw new NotSupportedException(string.Format("Filter {0} by property {1} is not supported", f.Type, f.Name));
                }
            }

            // Aggregate per redirect url
            DataResultEntity<TrackingStatPerUrlEntity> aggregation =
                await
                    _trackingStatRepository.AggregatePerUrlAsync(subscriptionId, redirectUrlFilter, dateFromFilter, dateToFilter, filter.OrderBy,
                        filter.OrderByDirection == OrderByDirections.Desc, filter.Skip, filter.Take, filter.Count);

            IEnumerable<DomainTrackingStatPerUrl> projectedResult = aggregation.Results.Select(r => _mapper.Map<TrackingStatPerUrlEntity, DomainTrackingStatPerUrl>(r));

            return new DataResult<DomainTrackingStatPerUrl>(projectedResult, aggregation.Count);
        }

        public async Task<DataResult<DomainTrackingStat>> GetUrlStatsAsync(string subscriptionId, string url, DataQueryOptions filter)
        {
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            IQueryable<TrackingStatEntity> data = _trackingStatRepository.Context.Where(s => s.SubscriptionId == subscriptionId && s.RedirectUrl == url);

            // Filter
            List<DataFilterRule> filters = filter.Filters.Where(f => f.Value != null).ToList();
            foreach (DataFilterRule dataFilterRule in filters)
            {
                DataFilterRule f = dataFilterRule;

                if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainTrackingStat>(x => x.Date),
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // by created date
                    var date = ((DateTime)f.Value).Date;
                    switch (f.Type)
                    {
                        case DataFilterTypes.Equal:
                            data = data.Where(p => p.Date > date && p.Date < date.AddDays(1));
                            break;
                        case DataFilterTypes.LessThan:
                            data = data.Where(p => p.Date < date);
                            break;
                        case DataFilterTypes.LessThanOrEqual:
                            data = data.Where(p => p.Date < date.AddDays(1));
                            break;
                        case DataFilterTypes.GreaterThan:
                            data = data.Where(p => p.Date > date);
                            break;
                        case DataFilterTypes.GreaterThanOrEqual:
                            data = data.Where(p => p.Date >= date);
                            break;
                    }
                }
                else
                {
                    throw new NotSupportedException(string.Format("Filter {0} by property {1} is not supported", f.Type, f.Name));
                }
            }

            // Sort
            if (!string.IsNullOrEmpty(filter.OrderBy))
            {
                if (string.Compare(filter.OrderBy, NameOfHelper.PropertyName<DomainTrackingStat>(x => x.Date),
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // order by name
                    data = filter.OrderByDirection == OrderByDirections.Asc
                        ? data.OrderBy(p => p.Date)
                        : data.OrderByDescending(p => p.Date);
                }
                else
                {
                    throw new NotSupportedException(string.Format("Ordering by {0} is not supported", filter.OrderBy));
                }
            }


            // Count of results
            long? count = null;
            if (filter.Count)
            {
                count = data.LongCount();
            }


            // Paging
            if (filter.Skip.HasValue)
            {
                data = data.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                data = data.Take(filter.Take.Value);
            }


            // post-processing
            IQueryable<DomainTrackingStat> results = data.Select(s => _mapper.Map<TrackingStatEntity, DomainTrackingStat>(s));

            return new DataResult<DomainTrackingStat>(results, count);
        }

        public async Task<IEnumerable<DomainTrackingStatPerDate>> GetStatsPerDateAsync(string subscriptionId, string url, DataQueryOptions filter)
        {
            if (string.IsNullOrEmpty(subscriptionId))
            {
                throw new ArgumentNullException("subscriptionId");
            }

            // Filter
            DateTime? dateFromFilter = null;
            DateTime? dateToFilter = null;
            List<DataFilterRule> filters = filter.Filters.Where(f => f.Value != null).ToList();
            foreach (DataFilterRule dataFilterRule in filters)
            {
                DataFilterRule f = dataFilterRule;

                if (string.Compare(f.Name, NameOfHelper.PropertyName<DomainTrackingStatPerDate>(x => x.Date),
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // by created date
                    DateTime date = ((DateTime)f.Value).Date;
                    switch (f.Type)
                    {
                        case DataFilterTypes.Equal:
                            dateFromFilter = date;
                            dateToFilter = date.AddDays(1);
                            break;
                        case DataFilterTypes.LessThan:
                            dateToFilter = date;
                            break;
                        case DataFilterTypes.LessThanOrEqual:
                            dateToFilter = date.AddDays(1);
                            break;
                        case DataFilterTypes.GreaterThan:
                            dateFromFilter = date.AddDays(1);
                            break;
                        case DataFilterTypes.GreaterThanOrEqual:
                            dateFromFilter = date;
                            break;
                    }
                }
                else
                {
                    throw new NotSupportedException(string.Format("Filter {0} by property {1} is not supported", f.Type, f.Name));
                }
            }

            // Aggregate per date
            IEnumerable<TrackingStatPerDateEntity> aggregation = await _trackingStatRepository.AggregatePerDateAsync(subscriptionId, url, dateFromFilter, dateToFilter);

            // Project
            IEnumerable<DomainTrackingStatPerDate> projectedResult = aggregation.Select(r => _mapper.Map<TrackingStatPerDateEntity, DomainTrackingStatPerDate>(r));

            return projectedResult;
        }
    }
}