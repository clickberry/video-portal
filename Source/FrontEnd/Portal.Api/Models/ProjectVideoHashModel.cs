// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public sealed class ProjectVideoHashModel
    {
        /// <summary>
        ///     Video hash name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidVideoFile")]
        [RegularExpression(@"^[A-Fa-f0-9]{40}$", ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "ProjectInvalidVideoHash")]
        public string Hash { get; set; }
    }
}