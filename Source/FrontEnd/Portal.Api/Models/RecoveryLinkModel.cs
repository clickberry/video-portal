// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace Portal.Api.Models
{
    public class RecoveryLinkModel
    {
        [Required]
        public string E { get; set; }

        [Required]
        public string I { get; set; }
    }
}