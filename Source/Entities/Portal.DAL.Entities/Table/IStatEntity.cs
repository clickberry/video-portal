// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Portal.DAL.Entities.Table
{
    public interface IStatEntity2
    {
        string Tick { get; set; }
    }
}