using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.RoleContext;

namespace Portal.Mappers.Mappings
{
    public sealed class PortalRoleMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<DomainRole, PortalRoleEntity>()
                  .ForMember(d => d.RoleName, o => o.MapFrom(s => s.RoleName));

            Mapper.CreateMap<PortalRoleEntity, DomainRole>()
                  .ForMember(d => d.RoleName, o => o.MapFrom(s => s.RoleName));
        }
    }
}