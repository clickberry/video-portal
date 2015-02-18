// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using AutoMapper;
using Portal.Domain.ProfileContext;
using Portal.Domain.SubscriptionContext;
using Portal.DTO.Subscriptions;

namespace Portal.Mappers.Mappings
{
    public sealed class ClientMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<Tuple<DomainUser, DomainCompany>, Client>()
                .ConvertUsing(tuple => new Client
                {
                    Email = tuple.Item1.Email,
                    ContactPerson = tuple.Item1.Name,
                    Country = tuple.Item2.Country,
                    CompanyName = tuple.Item2.Name,
                    Address = tuple.Item2.Address,
                    ZipCode = tuple.Item2.ZipCode,
                    PhoneNumber = tuple.Item2.Phone,
                    Ein = tuple.Item2.Ein
                });
        }
    }
}