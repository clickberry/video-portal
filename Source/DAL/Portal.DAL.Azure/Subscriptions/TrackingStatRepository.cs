// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoRepository;
using Portal.Common.Helpers;
using Portal.DAL.Entities.Table;
using Portal.DAL.Subscriptions;

namespace Portal.DAL.Azure.Subscriptions
{
    public class TrackingStatRepository : MongoRepository<TrackingStatEntity>, ITrackingStatRepository
    {
        public TrackingStatRepository(MongoUrl url) : base(url)
        {
        }

        public Task DeleteBySubscriptionIdAsync(string subscriptionId)
        {
            return Task.Run(() =>
            {
                IMongoQuery query = Query<TrackingStatEntity>.Where(t => t.SubscriptionId == subscriptionId);

                Collection.Remove(query);
            });
        }

        public Task<DataResultEntity<TrackingStatPerUrlEntity>> AggregatePerUrlAsync(string subscriptionId, string redirectUrl,
            DateTime? dateFrom, DateTime? dateTo, string orderBy, bool? orderDesc, long? skip, long? take, bool? count)
        {
            return Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(orderBy))
                {
                    PropertyInfo[] properties = typeof (TrackingStatPerUrlEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    if (properties.All(p => p.Name != orderBy))
                    {
                        throw new ArgumentException("Invalid property name for sort order", "orderBy");
                    }
                }

                if (skip.HasValue && skip.Value < 0)
                {
                    throw new ArgumentException("Skip value must be non-negative", "skip");
                }

                if (take.HasValue && take.Value <= 0)
                {
                    throw new ArgumentException("Take value must be positive", "take");
                }


                var filterPipeline = new List<BsonDocument>();

                string statSubscriptionIdPropertyName = NameOfHelper.PropertyName<TrackingStatEntity>(x => x.SubscriptionId);
                string statRedirectUrlPropertyName = NameOfHelper.PropertyName<TrackingStatEntity>(x => x.RedirectUrl);
                string statProjectIdPropertyName = NameOfHelper.PropertyName<TrackingStatEntity>(x => x.ProjectId);
                string statDatePropertyName = NameOfHelper.PropertyName<TrackingStatEntity>(x => x.Date);

                string statAggRedirectUrlPropertyName = NameOfHelper.PropertyName<TrackingStatPerUrlEntity>(x => x.RedirectUrl);
                string statAggProjectIdPropertyName = NameOfHelper.PropertyName<TrackingStatPerUrlEntity>(x => x.ProjectId);
                string statAggDateFromPropertyName = NameOfHelper.PropertyName<TrackingStatPerUrlEntity>(x => x.DateFrom);
                string statAggDateToPropertyName = NameOfHelper.PropertyName<TrackingStatPerUrlEntity>(x => x.DateTo);
                string statAggCountPropertyName = NameOfHelper.PropertyName<TrackingStatPerUrlEntity>(x => x.Count);


                // Filter
                filterPipeline.Add(new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                        {
                            { statSubscriptionIdPropertyName, subscriptionId }
                        }
                    }
                });

                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    filterPipeline.Add(new BsonDocument
                    {
                        {
                            "$match",
                            new BsonDocument
                            {
                                {
                                    statRedirectUrlPropertyName, new BsonDocument
                                    {
                                        { "$regex", string.Format("^{0}", Regex.Escape(redirectUrl)) }
                                    }
                                }
                            }
                        }
                    });
                }

                if (dateFrom.HasValue)
                {
                    DateTime dateFromUtc = DateTime.SpecifyKind(dateFrom.Value, DateTimeKind.Utc);
                    filterPipeline.Add(new BsonDocument
                    {
                        {
                            "$match",
                            new BsonDocument
                            {
                                {
                                    statDatePropertyName, new BsonDocument
                                    {
                                        { "$gte", dateFromUtc }
                                    }
                                }
                            }
                        }
                    });
                }

                if (dateTo.HasValue)
                {
                    DateTime dateToUtc = DateTime.SpecifyKind(dateTo.Value, DateTimeKind.Utc);
                    filterPipeline.Add(new BsonDocument
                    {
                        {
                            "$match",
                            new BsonDocument
                            {
                                {
                                    statDatePropertyName, new BsonDocument
                                    {
                                        { "$lte", dateToUtc }
                                    }
                                }
                            }
                        }
                    });
                }


                // Group
                var countGroupOperator = new BsonDocument
                {
                    {
                        "$group",
                        new BsonDocument
                        {
                            {
                                "_id", new BsonDocument
                                {
                                    { statAggRedirectUrlPropertyName, string.Format("${0}", statRedirectUrlPropertyName) },
                                    { statAggProjectIdPropertyName, string.Format("${0}", statProjectIdPropertyName) },
                                }
                            }
                        }
                    }
                };

                var resultGroupOperator = new BsonDocument
                {
                    {
                        "$group",
                        new BsonDocument
                        {
                            {
                                "_id", new BsonDocument
                                {
                                    { statAggRedirectUrlPropertyName, string.Format("${0}", statRedirectUrlPropertyName) },
                                    { statAggProjectIdPropertyName, string.Format("${0}", statProjectIdPropertyName) }
                                }
                            },
                            {
                                statAggCountPropertyName, new BsonDocument
                                {
                                    { "$sum", 1 }
                                }
                            },
                            {
                                statAggDateFromPropertyName, new BsonDocument
                                {
                                    { "$min", string.Format("${0}", statDatePropertyName) }
                                }
                            },
                            {
                                statAggDateToPropertyName, new BsonDocument
                                {
                                    { "$max", string.Format("${0}", statDatePropertyName) }
                                }
                            }
                        }
                    }
                };


                // Project for compatible object
                var projectOperator = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                        {
                            { "_id", 0 },
                            { statAggRedirectUrlPropertyName, string.Format("$_id.{0}", statAggRedirectUrlPropertyName) },
                            { statAggProjectIdPropertyName, string.Format("$_id.{0}", statAggProjectIdPropertyName) },
                            { statAggCountPropertyName, 1 },
                            { statAggDateFromPropertyName, 1 },
                            { statAggDateToPropertyName, 1 }
                        }
                    }
                };

                // Sort
                BsonDocument sortOperator = null;
                if (!string.IsNullOrEmpty(orderBy))
                {
                    sortOperator = new BsonDocument
                    {
                        { "$sort", new BsonDocument { { orderBy, orderDesc.HasValue && orderDesc.Value ? -1 : 1 } } }
                    };
                }

                // Skip
                BsonDocument skipOperator = null;
                if (skip.HasValue)
                {
                    skipOperator = new BsonDocument
                    {
                        { "$skip", skip.Value }
                    };
                }

                // Limit
                BsonDocument limitOperator = null;
                if (take.HasValue)
                {
                    limitOperator = new BsonDocument
                    {
                        { "$limit", take.Value }
                    };
                }

                // Build pipelines
                var countPipeline = new List<BsonDocument>(filterPipeline) { countGroupOperator };

                var resultPipeline = new List<BsonDocument>(filterPipeline) { resultGroupOperator, projectOperator };
                if (sortOperator != null)
                {
                    resultPipeline.Add(sortOperator);
                }
                if (skipOperator != null)
                {
                    resultPipeline.Add(skipOperator);
                }
                if (limitOperator != null)
                {
                    resultPipeline.Add(limitOperator);
                }

                // Execute aggregations

                // Count
                var aggArgs = new AggregateArgs { Pipeline = countPipeline };
                long? resultCount = null;
                if (count.HasValue && count.Value)
                {
                    resultCount = Collection.Aggregate(aggArgs).LongCount();
                }

                // Result
                aggArgs.Pipeline = resultPipeline;
                IEnumerable<BsonDocument> aggResult = Collection.Aggregate(aggArgs);

                // Project
                IEnumerable<TrackingStatPerUrlEntity> result = aggResult.Select(BsonSerializer.Deserialize<TrackingStatPerUrlEntity>);

                return new DataResultEntity<TrackingStatPerUrlEntity>(result, resultCount);
            });
        }

        public Task<IEnumerable<TrackingStatPerDateEntity>> AggregatePerDateAsync(string subscriptionId, string redirectUrl, DateTime? dateFrom, DateTime? dateTo)
        {
            return Task.Run(() =>
            {
                var filterPipeline = new List<BsonDocument>();

                string statSubscriptionIdPropertyName = NameOfHelper.PropertyName<TrackingStatEntity>(x => x.SubscriptionId);
                string statRedirectUrlPropertyName = NameOfHelper.PropertyName<TrackingStatEntity>(x => x.RedirectUrl);
                string statDatePropertyName = NameOfHelper.PropertyName<TrackingStatEntity>(x => x.Date);

                string statAggDatePropertyName = NameOfHelper.PropertyName<TrackingStatPerDateEntity>(x => x.Date);
                string statAggCountPropertyName = NameOfHelper.PropertyName<TrackingStatPerDateEntity>(x => x.Count);


                // Filter
                filterPipeline.Add(new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                        {
                            { statSubscriptionIdPropertyName, subscriptionId }
                        }
                    }
                });

                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    filterPipeline.Add(new BsonDocument
                    {
                        {
                            "$match",
                            new BsonDocument
                            {
                                {
                                    statRedirectUrlPropertyName, new BsonDocument
                                    {
                                        { "$regex", string.Format("^{0}", Regex.Escape(redirectUrl)) }
                                    }
                                }
                            }
                        }
                    });
                }

                if (dateFrom.HasValue)
                {
                    DateTime dateFromUtc = DateTime.SpecifyKind(dateFrom.Value, DateTimeKind.Utc);
                    filterPipeline.Add(new BsonDocument
                    {
                        {
                            "$match",
                            new BsonDocument
                            {
                                {
                                    statDatePropertyName, new BsonDocument
                                    {
                                        { "$gte", dateFromUtc }
                                    }
                                }
                            }
                        }
                    });
                }

                if (dateTo.HasValue)
                {
                    DateTime dateToUtc = DateTime.SpecifyKind(dateTo.Value, DateTimeKind.Utc);
                    filterPipeline.Add(new BsonDocument
                    {
                        {
                            "$match",
                            new BsonDocument
                            {
                                {
                                    statDatePropertyName, new BsonDocument
                                    {
                                        { "$lt", dateToUtc }
                                    }
                                }
                            }
                        }
                    });
                }


                // Group by date
                // Since we have no way to combine datetime parts back to ISODate object we should substract all time parts from ISODate object before grouping
                // http://www.kamsky.org/1/post/2013/03/stupid-date-tricks-with-aggregation-framework.html

                string refDateProperty = string.Format("${0}", statDatePropertyName);
                var preGroupOperator = new BsonDocument
                {
                    {
                        "$project", new BsonDocument
                        {
                            {
                                statDatePropertyName, new BsonDocument
                                {
                                    {
                                        "$subtract", new BsonArray
                                        {
                                            string.Format("${0}", statDatePropertyName),
                                            new BsonDocument
                                            {
                                                {
                                                    "$add", new BsonArray
                                                    {
                                                        new BsonDocument
                                                        {
                                                            { "$millisecond", refDateProperty }
                                                        },
                                                        new BsonDocument
                                                        {
                                                            { "$multiply", new BsonArray { new BsonDocument { { "$second", refDateProperty } }, 1000 } }
                                                        },
                                                        new BsonDocument
                                                        {
                                                            { "$multiply", new BsonArray { new BsonDocument { { "$minute", refDateProperty } }, 60, 1000 } }
                                                        },
                                                        new BsonDocument
                                                        {
                                                            { "$multiply", new BsonArray { new BsonDocument { { "$hour", refDateProperty } }, 60, 60, 1000 } }
                                                        },
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };

                var groupOperator = new BsonDocument
                {
                    {
                        "$group",
                        new BsonDocument
                        {
                            {
                                "_id", new BsonDocument
                                {
                                    {
                                        statAggDatePropertyName, refDateProperty
                                    }
                                }
                            },
                            {
                                statAggCountPropertyName, new BsonDocument
                                {
                                    { "$sum", 1 }
                                }
                            }
                        }
                    }
                };


                // Project for compatible object
                var projectOperator = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                        {
                            { "_id", 0 },
                            { statAggDatePropertyName, string.Format("$_id.{0}", statAggDatePropertyName) },
                            { statAggCountPropertyName, 1 }
                        }
                    }
                };

                // Sort
                var sortOperator = new BsonDocument
                {
                    { "$sort", new BsonDocument { { statAggDatePropertyName, 1 } } }
                };


                // Build pipelines
                var resultPipeline = new List<BsonDocument>(filterPipeline) { preGroupOperator, groupOperator, projectOperator, sortOperator };

                // Execute aggregations
                var aggArgs = new AggregateArgs { Pipeline = resultPipeline };
                IEnumerable<BsonDocument> aggResult = Collection.Aggregate(aggArgs);

                // Project
                IEnumerable<TrackingStatPerDateEntity> result = aggResult.Select(BsonSerializer.Deserialize<TrackingStatPerDateEntity>);

                return result;
            });
        }

        public Task<TrackingStatEntity> GetAsync(TrackingStatEntity entity)
        {
            return Task.Run(() => Context.FirstOrDefault(s => s.Id == entity.Id));
        }
    }
}