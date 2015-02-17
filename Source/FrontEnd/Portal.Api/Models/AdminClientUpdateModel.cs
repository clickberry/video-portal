// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Portal.Domain;
using Portal.Resources.Api;

namespace Portal.Api.Models
{
    public class AdminClientUpdateModel
    {
        [EnumDataType(typeof (ResourceState), ErrorMessageResourceType = typeof (ResponseMessages), ErrorMessageResourceName = "InvalidResourceState")]
        public ResourceState State { get; set; }
    }
}