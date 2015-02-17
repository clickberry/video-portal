// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Configuration;
using Portal.BLL.Services;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.DAL.Factories;
using Portal.DAL.Mailer;
using Portal.DAL.User;
using Portal.Domain.AccountContext;
using Portal.Domain.MailerContext;
using Portal.Domain.ProfileContext;
using Portal.Exceptions.CRUD;
using Portal.Resources.Emails;
using PortalResources = Portal.Resources.Properties.Resources;

namespace Portal.BLL.Concrete.Services
{
    public class PasswordRecoveryService : IPasswordRecoveryService
    {
        private readonly TimeSpan _expirationTime = TimeSpan.FromMinutes(15);
        private readonly IMailerRepository _mailerRepository;
        private readonly ITableRepository<PasswordRecoveryEntity> _passwordRecoverRepository;
        private readonly IPasswordRecoveryFactory _passwordRecoveryFactory;
        private readonly IPasswordService _passwordService;
        private readonly IRecoveryLinkService _recoveryLinkService;
        private readonly IPortalFrontendSettings _settings;
        private readonly IUserRepository _userRepository;

        public PasswordRecoveryService(
            IRepositoryFactory repositoryFactory,
            IUserRepository userRepository,
            IMailerRepository mailerRepository,
            IPasswordRecoveryFactory passwordRecoveryFactory,
            IPortalFrontendSettings settings,
            IRecoveryLinkService recoveryLinkService,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordRecoveryFactory = passwordRecoveryFactory;
            _settings = settings;
            _recoveryLinkService = recoveryLinkService;
            _passwordService = passwordService;
            _mailerRepository = mailerRepository;
            _passwordRecoverRepository = repositoryFactory.Create<PasswordRecoveryEntity>();
        }

        public async Task SendNewRecoveryMail(DomainUser user, string validationPath)
        {
            string guid = Guid.NewGuid().ToString();
            DateTime expires = DateTime.UtcNow.Add(_expirationTime);
            var recoveryLink = new RecoveryLink { ExpirationDate = expires, Id = guid };

            PasswordRecoveryEntity entity = _passwordRecoveryFactory.CreateDefault(user.Id, guid, user.Email, expires);
            PasswordRecoveryEntity recoveryEntity = await _passwordRecoverRepository.AddAsync(entity);

            string linkRoot = _settings.PortalUri + validationPath;
            string linkText = _recoveryLinkService.CreateRecoveryLinkText(recoveryLink, linkRoot);

            Email emailToSend = ComposeRecoveryMail(recoveryEntity, user.Name, linkText);
            await _mailerRepository.SendMail(emailToSend);
        }

        public RecoveryLink GetLink(string encryptedExpiration, string encryptedLinkId)
        {
            return _recoveryLinkService.DecryptParameters(encryptedExpiration, encryptedLinkId);
        }

        public async Task ChangePassword(RecoveryLink recoveryLink, string newPassword)
        {
            PasswordRecoveryEntity entity = await _passwordRecoverRepository.SingleOrDefaultAsync(e => e.LinkData == recoveryLink.Id);
            if (entity == null || entity.IsConfirmed)
            {
                throw new NotFoundException();
            }

            entity.Modified = DateTime.UtcNow;
            entity.IsConfirmed = true;
            entity = await _passwordRecoverRepository.UpdateAsync(entity);

            UserEntity user = await _userRepository.FindByEmailAsync(entity.Email);
            if (user == null)
            {
                throw new NotFoundException();
            }

            await _passwordService.ChangePasswordAsync(user.Id, newPassword);
        }

        public async Task<bool> CheckIfLinkIsValid(RecoveryLink recoveryLink)
        {
            PasswordRecoveryEntity entity = await _passwordRecoverRepository.SingleOrDefaultAsync(e => e.LinkData == recoveryLink.Id);
            return entity != null && !entity.IsConfirmed;
        }

        private Email ComposeRecoveryMail(PasswordRecoveryEntity recoveryEntity, string userName, string link)
        {
            string emailAddress = _settings.EmailAddressAlerts;
            string emailDisplayName = Emails.SenderDisplayName;
            string emailSubject = Emails.SubjectPasswordRecovery;
            string emailTemplate = PortalResources.UserRecovery;
            string emailBody = String.Format(emailTemplate, userName, link);

            var result = new Email(emailSubject, emailBody, ContentType.Html, new EmailAddress(emailAddress, emailDisplayName));
            result.To.Add(new EmailAddress(recoveryEntity.Email, recoveryEntity.Email));

            return result;
        }
    }
}