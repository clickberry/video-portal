// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Portal.BLL.Services;
using Portal.DAL.Context;
using Portal.DAL.Entities.Table;
using Portal.DAL.Mailer;
using Portal.Domain.MailerContext;
using Portal.Mappers;

namespace Portal.BLL.Concrete.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IMailerRepository _mailerRepository;
        private readonly IMapper _mapper;
        private readonly ITableRepository<SendEmailEntity> _sendEmailRepository;

        public EmailSenderService(
            IRepositoryFactory repositoryFactory,
            IMailerRepository mailerRepository,
            IMapper mapper)
        {
            _mailerRepository = mailerRepository;
            _mapper = mapper;
            _sendEmailRepository = repositoryFactory.Create<SendEmailEntity>();
        }

        public async Task<SendEmailDomain> SendAndLogEmailAsync(SendEmailDomain email)
        {
            string emailAddress = email.Address;
            string emailDisplayName = email.DisplayName;

            // Send email
            var emailToSend = new Email(email.Subject, email.Body, ContentType.Html, new EmailAddress(emailAddress, emailDisplayName));
            emailToSend.To.Add(new EmailAddress(email.Emails.First(), emailDisplayName)); //Email must always have a valid recipient
            emailToSend.Headers.AddRange(email.Emails); // Must always add to headers for utilizing send-grid api

            await _mailerRepository.SendMail(emailToSend);

            email.Id = ((int)email.Template) + GetUtcTimestamp();
            email.Created = DateTime.UtcNow;

            // Log sent email
            SendEmailEntity entity = _mapper.Map<SendEmailDomain, SendEmailEntity>(email);
            await _sendEmailRepository.AddAsync(entity);

            return email;
        }

        public async Task<SendEmailDomain> SendEmailAsync(SendEmailDomain email)
        {
            string emailAddress = email.Address;
            string emailDisplayName = email.DisplayName;

            // Send email
            var emailToSend = new Email(email.Subject, email.Body, ContentType.Html, new EmailAddress(emailAddress, emailDisplayName));
            emailToSend.To.Add(new EmailAddress(email.Emails.First(), emailDisplayName)); //Email must always have a valid recipient
            emailToSend.Headers.AddRange(email.Emails); // Must always add to headers for utilizing send-grid api

            await _mailerRepository.SendMail(emailToSend);

            return email;
        }

        private static string GetUtcTimestamp()
        {
            return (DateTime.UtcNow.ToString("yyyy_MM_dd_") + ((DateTime.MaxValue - DateTime.UtcNow).Ticks).ToString(CultureInfo.InvariantCulture)) + Guid.NewGuid();
        }
    }
}