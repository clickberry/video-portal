// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.DTO.Common;
using Portal.DTO.Files;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    /// <summary>
    ///     Project model.
    /// </summary>
    public sealed class FileModel : UploadFileData
    {
        /// <summary>
        ///     Name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "FileInvalidName")]
        [StringLength(256, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Name { get; set; }

        /// <summary>
        ///     Data file name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "FileInvalidFile")]
        [ImageFile(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "FileInvalidFormat")]
        public override FileEntity File { get; set; }
    }
}