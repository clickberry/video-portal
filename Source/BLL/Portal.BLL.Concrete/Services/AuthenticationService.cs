// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.BLL.Services;
using Portal.DAL.Authentication;
using Portal.DAL.Entities.Table;
using Portal.DAL.User;
using Portal.Domain.ProfileContext;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticator _authenticator;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IAuthenticator authenticator, IUserRepository userRepository, IMapper mapper)
        {
            _authenticator = authenticator;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<string> SetUserAsync(DomainUser user, TokenData tokenData, bool isPersistent = false)
        {
            if (tokenData != null && tokenData.IdentityProvider != ProviderType.Email)
            {
                await _userRepository.UpdateMembershipAsync(user.Id, _mapper.Map<TokenData, UserMembershipEntity>(tokenData));
            }

            string identityProvider = tokenData == null ? null : tokenData.IdentityProvider.ToString();
            return _authenticator.Authenticate(_mapper.Map<DomainUser, UserEntity>(user), identityProvider, isPersistent);
        }

        public async Task SetDataAsync(string data)
        {
            string userId = _authenticator.GetUserNameFromValue(data);
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            UserEntity user = await _userRepository.GetAsync(userId);
            _authenticator.Authenticate(user, null);
        }

        public void UpdateIdentityClaims(DomainUser user)
        {
            _authenticator.UpdateIdentityClaims(user.Id, user.Email, user.Name, null);
        }

        public void Clear()
        {
            _authenticator.Clear();
        }
    }
}