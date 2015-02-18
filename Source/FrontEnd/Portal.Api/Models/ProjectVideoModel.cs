// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.DTO.Common;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public sealed class ProjectVideoModel
    {
        /// <summary>
        ///     Video file name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidVideoFile")]
        [VideoFile(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidVideoFile")]
        public FileEntity Video { get; set; }
    }
}