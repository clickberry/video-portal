// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.DTO.Portal;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public class PortalRoleModel : PortalRole
    {
        [Required]
        [StringLength(64, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Name { get; set; }
    }
}