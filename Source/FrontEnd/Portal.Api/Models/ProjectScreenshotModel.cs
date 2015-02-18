// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.DTO.Common;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    /// <summary>
    ///     Project screenshot model.
    /// </summary>
    public sealed class ProjectScreenshotModel
    {
        /// <summary>
        ///     Screenshot file.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidAvsxFile")]
        [ImageFile(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidScreenshotFile")]
        public FileEntity Screenshot { get; set; }
    }
}