// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;
using Portal.DAL.Entities.Table;
using Portal.Domain.MailerContext;

namespace Portal.Mappers.Mappings
{
    public class SendEmailMapping : IMapping
    {
        public void Register()
        {
            Mapper.CreateMap<SendEmailDomain, SendEmailEntity>()
                .ConvertUsing(
                    sendEmail => new SendEmailEntity
                    {
                        Id = sendEmail.Id,
                        UserId = sendEmail.UserId,
                        Emails = JsonConvert.SerializeObject(sendEmail.Emails),
                        Subject = sendEmail.Subject,
                        Created = sendEmail.Created
                    });
            Mapper.CreateMap<SendEmailEntity, SendEmailDomain>()
                .ConvertUsing(
                    entity => new SendEmailDomain
                    {
                        Id = entity.Id,
                        Emails = JsonConvert.DeserializeObject<List<string>>(entity.Emails),
                        Subject = entity.Subject,
                        UserId = entity.UserId,
                        Created = entity.Created
                    });
        }
    }
}