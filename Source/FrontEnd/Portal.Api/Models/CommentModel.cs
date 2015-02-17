// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.DTO.Projects;

namespace Portal.Api.Models
{
    public sealed class CommentModel : Comment
    {
        [Required]
        [StringLength(4096)]
        public override string Body { get; set; }
    }
}