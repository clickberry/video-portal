// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using Portal.Domain.Admin;
using Portal.DTO.Common;

namespace Portal.Web.Models
{
    [Obsolete("Remove after migration to admin API")]
    public class FilterRulesModel : FilterRules
    {
        [Required]
        [Range(1, 100)]
        public override int Take { get; set; }

        [Required]
        public override int? Skip { get; set; }

        [StringLength(100)]
        public override string Orderby { get; set; }

        public override OrderByDirections OrderDirection { get; set; }

        [StringLength(1024)]
        public override string Value { get; set; }
    }
}