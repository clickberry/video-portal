// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using AutoMapper;
using Portal.Domain.Admin;
using Portal.DTO.Admin;

namespace Portal.Mappers.Mappings
{
    public sealed class UserForAdminMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<AdminUser, DomainUserForAdmin>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(source => source.Id))
                ;

            Mapper.CreateMap<DomainUserForAdmin, AdminUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.UserId))
                ;
        }
    }
}