// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Asp.Infrastructure.Validation;
using Portal.Domain.ProjectContext;
using Portal.DTO.Common;
using Portal.DTO.Projects;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    /// <summary>
    ///     External project model.
    /// </summary>
    [DataContract]
    public sealed class ExternalProjectModel : Project
    {
        public ExternalProjectModel()
        {
            EnableComments = true;
        }

        /// <summary>
        ///     Gets or sets a project name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidName")]
        [StringLength(256, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Name { get; set; }

        /// <summary>
        ///     Gets or sets a project description.
        /// </summary>
        [NonWhiteSpaceString(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidDescription")]
        [StringLength(32768, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Description { get; set; }

        /// <summary>
        ///     Gets or sets a project type.
        /// </summary>
        [EnumDataType(typeof (ProjectType), ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidType")]
        public override ProjectType ProjectType { get; set; }

        /// <summary>
        ///     Gets or sets a project subtype.
        /// </summary>
        [EnumDataType(typeof (ProjectSubtype), ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidSubtype")]
        public override ProjectSubtype ProjectSubtype { get; set; }

        /// <summary>
        ///     Gets or sets a project access.
        /// </summary>
        [EnumDataType(typeof (ProjectAccess), ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidAccess")]
        public override ProjectAccess Access { get; set; }

        /// <summary>
        ///     Gets or sets an avsx file.
        /// </summary>
        [AvsxFile(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidAvsxFile")]
        public FileEntity Avsx { get; set; }

        /// <summary>
        ///     Gets or sets a screenshot file.
        /// </summary>
        [ImageFile(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidScreenshotFile")]
        public FileEntity Screenshot { get; set; }

        /// <summary>
        ///     Gets or sets a video product name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ExternalVideoInvalidProduct")]
        [StringLength(256, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public string ProductName { get; set; }

        /// <summary>
        ///     Gets or sets a video uri.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ExternalVideoInvalidVideoUri")]
        [StringLength(1011, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        [Url]
        public string VideoUri { get; set; }
    }
}