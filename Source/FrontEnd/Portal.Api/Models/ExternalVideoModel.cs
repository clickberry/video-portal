// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.Domain.ProjectContext;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public sealed class ExternalVideoModel : DomainExternalVideo
    {
        /// <summary>
        ///     Product name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ExternalVideoInvalidProduct")]
        [StringLength(256, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        public override string ProductName { get; set; }

        /// <summary>
        ///     Video Uri.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ExternalVideoInvalidVideoUri")]
        [StringLength(1011, ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ValidationValueMustNotBeGreaterThan")]
        [Url]
        public override string VideoUri { get; set; }
    }
}