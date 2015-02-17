// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Portal.Domain.MailerContext;

namespace Portal.BLL.Services
{
    public interface IEmailSenderService
    {
        Task<SendEmailDomain> SendAndLogEmailAsync(SendEmailDomain email);
        Task<SendEmailDomain> SendEmailAsync(SendEmailDomain email);
    }
}