// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;

namespace Portal.Api.Models
{
    public class ClientSubscriptionStatUrlModel
    {
        [Url]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [NonWhiteSpaceString]
        public string Url { get; set; }
    }
}