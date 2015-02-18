// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Web.Http.OData.Query;
using AutoMapper;
using Portal.Domain;
using Portal.Mappers.TypeConverters;

namespace Portal.Mappers.Mappings
{
    public sealed class DataQueryOptionsMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<ODataQueryOptions, DataQueryOptions>().ConvertUsing(new ODataQueryOptionsToDataQueryOptions());
        }
    }
}