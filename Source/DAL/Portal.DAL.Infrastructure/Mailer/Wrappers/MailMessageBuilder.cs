// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using Portal.DAL.Mailer;
using Portal.Domain.MailerContext;
using ContentType = Portal.Domain.MailerContext.ContentType;

namespace Portal.DAL.Infrastructure.Mailer.Wrappers
{
    public class MailMessageBuilder : IEmailBuilder, IDisposable
    {
        public const string SendGridApiHeader = "X-Smtpapi";
        private readonly MailMessage _mailMessage;


        public MailMessageBuilder()
        {
            _mailMessage = new MailMessage();
        }

        public void AddToWithHeader(List<string> emailAddresses)
        {
            string headerValue = JsonConvert.SerializeObject(new
            {
                to = emailAddresses
            }, Formatting.Indented);
            _mailMessage.Headers.Add(SendGridApiHeader, headerValue);
        }

        public void SetSender(EmailAddress emailAddress)
        {
            _mailMessage.From = new MailAddress(emailAddress.Address, emailAddress.DisplayName, Encoding.UTF8);
        }

        public void AddToRecipient(EmailAddress emailAddress)
        {
            _mailMessage.To.Add(new MailAddress(emailAddress.Address, emailAddress.DisplayName, Encoding.UTF8));
        }

        public void AddCcRecipient(EmailAddress emailAddress)
        {
            _mailMessage.CC.Add(new MailAddress(emailAddress.Address, emailAddress.DisplayName, Encoding.UTF8));
        }

        public void AddBccRecipient(EmailAddress emailAddress)
        {
            _mailMessage.Bcc.Add(new MailAddress(emailAddress.Address, emailAddress.DisplayName, Encoding.UTF8));
        }

        public void SetContent(string subject, string content, string contentType)
        {
            if (contentType == ContentType.Html)
            {
                _mailMessage.IsBodyHtml = true;
                _mailMessage.BodyTransferEncoding = TransferEncoding.SevenBit;
            }
            _mailMessage.Subject = subject;
            _mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(content, Encoding.UTF8, contentType));
        }

        public MailMessage GetResult()
        {
            return _mailMessage;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _mailMessage.Dispose();
        }

        #endregion
    }
}