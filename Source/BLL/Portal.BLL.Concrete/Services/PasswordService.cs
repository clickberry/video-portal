// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Portal.BLL.Infrastructure;
using Portal.BLL.Services;
using Portal.DAL.Entities.Table;
using Portal.DAL.User;
using Portal.Exceptions.CRUD;

namespace Portal.BLL.Concrete.Services
{
    public sealed class PasswordService : IPasswordService
    {
        private readonly ICryptoService _cryptoService;
        private readonly IUserRepository _userRepository;

        public PasswordService(IUserRepository userRepository, ICryptoService cryptoService)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string newPassword, string oldPassword)
        {
            bool passwordMatch = false;
            await ChangePassword(userId, newPassword, s =>
            {
                string password = _cryptoService.EncodePassword(oldPassword, s.PasswordSalt);
                passwordMatch = password == s.Password;
                return passwordMatch;
            });

            return passwordMatch;
        }

        public Task ChangePasswordAsync(string userId, string newPassword)
        {
            return ChangePassword(userId, newPassword, entity => true);
        }

        public async Task SetPasswordAsync(string userId, string passwordHash, string passwordSalt)
        {
            UserEntity user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException(string.Format("User '{0}' was not found.", userId));
            }

            // Set password
            user.Password = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.UpdateAsync(user);
        }

        private async Task ChangePassword(string userId, string newPassword, Predicate<UserEntity> checkPassword)
        {
            UserEntity user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException(string.Format("User '{0}' was not found.", userId));
            }

            // Check password
            if (!checkPassword(user))
            {
                throw new BadRequestException("Invalid password.");
            }

            // Change password
            user.PasswordSalt = _cryptoService.GenerateSalt();
            user.Password = _cryptoService.EncodePassword(newPassword, user.PasswordSalt);

            await _userRepository.UpdateAsync(user);
        }
    }
}