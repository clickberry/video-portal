// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.Domain.MailerContext;

namespace Portal.DAL.Mailer
{
    public interface IEmailInitializer
    {
        void Initialize(IEmailBuilder emailBuilder, EmailBase email);
    }
}