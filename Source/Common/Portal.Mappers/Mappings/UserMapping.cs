// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Linq;
using AutoMapper;
using Portal.DAL.Entities.Table;
using Portal.Domain.ProfileContext;
using Portal.Domain.ProjectContext;
using Profile = Portal.DTO.User.Profile;

namespace Portal.Mappers.Mappings
{
    public sealed class UserMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<UserEntity, DomainUser>()
                .ForMember(d => d.ApplicationName, o => o.MapFrom(s => s.AppName))
                .ForMember(d => d.UserAgent, o => o.MapFrom(s => (ProductType)s.ProductId))
                .ForMember(d => d.UsedStorageSpace, o => o.Ignore())
                ;

            Mapper.CreateMap<DomainUser, UserEntity>()
                .ForMember(d => d.AppName, o => o.MapFrom(s => s.ApplicationName))
                .ForMember(d => d.ProductId, o => o.MapFrom(s => (int)s.UserAgent))
                .ForMember(d => d.NameSort, o => o.MapFrom(s => s.Name != null ? s.Name.ToLowerInvariant() : null))
                .ForMember(d => d.Password, o => o.Ignore())
                .ForMember(d => d.PasswordSalt, o => o.Ignore())
                ;


            // Memberships

            Mapper.CreateMap<UserMembershipEntity, UserMembership>()
                .ForMember(d => d.IdentityProvider,
                    o => o.MapFrom(s => (ProviderType)Enum.Parse(typeof (ProviderType), s.IdentityProvider)))
                ;

            Mapper.CreateMap<UserMembership, UserMembershipEntity>()
                .ForMember(d => d.IdentityProvider, o => o.MapFrom(s => s.IdentityProvider.ToString()))
                ;


            // Followers

            Mapper.CreateMap<FollowerEntity, Follower>()
                ;

            Mapper.CreateMap<Follower, FollowerEntity>()
                ;

            Mapper.CreateMap<DomainUser, Follower>()
                ;


            // DTO

            Mapper.CreateMap<DomainUser, Profile>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.MaximumDiskSpace, o => o.MapFrom(s => s.MaximumStorageSpace))
                .ForMember(d => d.UsedDiskSpace, o => o.MapFrom(s => s.UsedStorageSpace))
                .ForMember(d => d.Memberships, o => o.MapFrom(s => s.Memberships.Select(m => m.IdentityProvider)))
                .ForMember(d => d.AvatarUrl, o => o.Ignore())
                ;

            // Tokens
            Mapper.CreateMap<TokenData, UserMembershipEntity>();
        }
    }
}