// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Portal.Domain.ProjectContext;
using Portal.DTO.Projects;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    /// <summary>
    ///     Project model.
    /// </summary>
    public sealed class ProjectModel : Project
    {
        public ProjectModel()
        {
            EnableComments = true;
        }

        /// <summary>
        ///     Name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidName")]
        [StringLength(256, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Name { get; set; }

        /// <summary>
        ///     Description.
        /// </summary>
        [NonWhiteSpaceString(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidDescription")]
        [StringLength(32768, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string Description { get; set; }

        [EnumDataType(typeof (ProjectAccess), ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidAccess")]
        public override ProjectAccess Access { get; set; }

        [EnumDataType(typeof (ProjectType), ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidType")]
        public override ProjectType ProjectType { get; set; }

        [EnumDataType(typeof (ProjectSubtype), ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidSubtype")]
        public override ProjectSubtype ProjectSubtype { get; set; }

        public override bool EnableComments { get; set; }
    }
}