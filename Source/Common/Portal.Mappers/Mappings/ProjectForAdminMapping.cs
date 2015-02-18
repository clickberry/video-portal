// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using AutoMapper;
using Portal.Domain.Admin;
using Portal.DTO.Admin;

namespace Portal.Mappers.Mappings
{
    public sealed class ProjectForAdminMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<AdminProject, DomainProjectForAdmin>()
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(dest => dest.Modified, opt => opt.Ignore())
                ;

            Mapper.CreateMap<DomainProjectForAdmin, AdminProject>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.ProjectId))
                ;
        }
    }
}