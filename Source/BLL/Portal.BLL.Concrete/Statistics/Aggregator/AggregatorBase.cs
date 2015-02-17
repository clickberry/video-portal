// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.DAL.Context;
using Portal.Mappers.Statistics;
using Wrappers.Interface;

namespace Portal.BLL.Concrete.Statistics.Aggregator
{
    public abstract class AggregatorBase
    {
        protected AggregatorBase(IRepositoryFactory repositoryFactory, IStatEntityFactory statEntityFactory, IGuidWrapper guid, IDateTimeWrapper dateTime)
        {
            RepositoryFactory = repositoryFactory;
            StatEntityFactory = statEntityFactory;
            GuidWraper = guid;
            DateTimeWrapper = dateTime;
        }

        protected IRepositoryFactory RepositoryFactory { get; private set; }

        protected IStatEntityFactory StatEntityFactory { get; private set; }

        protected IGuidWrapper GuidWraper { get; private set; }

        protected IDateTimeWrapper DateTimeWrapper { get; private set; }
    }
}