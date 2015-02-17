// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using AutoMapper;
using LinkTracker.Mappings.Entities;

namespace LinkTracker.Mappings
{
    public class MappingEngine : IMappingEngine
    {
        public MappingEngine()
        {
            var mappings = new IMapping[]
            {
                new TrackingUrlMapping(),
                new TrackingStatMapping()
            };

            RegisterMappings(mappings);

            Mapper.AssertConfigurationIsValid();
        }

        public TOut Map<TIn, TOut>(TIn source)
        {
            return Mapper.Map<TIn, TOut>(source);
        }

        private static void RegisterMappings(IEnumerable<IMapping> mappings)
        {
            foreach (IMapping mapping in mappings)
            {
                mapping.Register();
            }
        }
    }
}