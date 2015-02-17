// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoRepository;
using Portal.DAL.Entities.Table;

namespace Portal.DAL.Azure.Queries
{
    public static class QueryFactory
    {
        private static readonly Dictionary<Type, object> Dictionary;

        static QueryFactory()
        {
            Dictionary = new Dictionary<Type, object>
            {
                { typeof (PasswordRecoveryEntity), new PasswordRecoveryQuery() },
                { typeof (ProcessedScreenshotEntity), new ProcessedScreenshotQuery() },
                { typeof (ProcessedVideoEntity), new ProcessedVideoQuery() },
                { typeof (StandardReportV3Entity), new StandardReportQuery() },
                { typeof (FileEntity), new FileQuery() },
                { typeof (ProjectEntity), new ProjectQuery() },
                { typeof (VideoQueueEntity), new VideoQueueQuery() },
            };
        }

        public static IMongoQuery Create<T>(T entity) where T : IEntity
        {
            var query = Dictionary[typeof (T)] as IQuery<T>;
            if (query == null)
            {
                throw new ArgumentException(String.Format("Query for the type of {0} is not exist.", typeof (T)));
            }

            return query.Create(entity);
        }
    }
}