// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MongoRepository;

namespace LinkTracker.DAL.Entities
{
    [CollectionName("Sequence")]
    public class SequenceEntity : Entity
    {
        public string Name { get; set; }

        public long Current { get; set; }
    }
}