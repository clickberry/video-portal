// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;

namespace Portal.Mappers
{
    public interface IMapper
    {
        TOut Map<TIn, TOut>(TIn source);

        IQueryable<TOut> Project<TIn, TOut>(IQueryable<TIn> source);
    }
}