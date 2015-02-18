// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Portal.DAL.Mailer
{
    public interface IMailClient
    {
        Task SendAsync(IEmailBuilder emailBuilder);
    }
}