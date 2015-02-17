// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Asp.Infrastructure.Attributes;
using Configuration;
using Portal.Api.Models;
using Portal.BLL.Services;
using Portal.Domain.MailerContext;
using Portal.Resources.Emails;

namespace Portal.Api.Controllers
{
    [AuthorizeHttp]
    [ValidationHttp]
    [Route("email")]
    public sealed class EmailController : ApiControllerBase
    {
        private readonly IEmailSenderService _emailSender;
        private readonly IPortalFrontendSettings _settings;

        public EmailController(IEmailSenderService emailSender, IPortalFrontendSettings settings)
        {
            _emailSender = emailSender;
            _settings = settings;
        }

        // POST /api/email
        public async Task<HttpResponseMessage> Post(SendEmailModel sendEmailModel)
        {
            var email = new SendEmailDomain
            {
                Address = _settings.EmailAddressInfo,
                DisplayName = Emails.SenderDisplayName,
                UserId = UserId,
                Body = sendEmailModel.Body,
                Emails = sendEmailModel.Emails,
                Subject = sendEmailModel.Subject
            };

            await _emailSender.SendEmailAsync(email);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}