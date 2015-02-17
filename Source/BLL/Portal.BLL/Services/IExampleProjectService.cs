// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Domain;
using Portal.DTO.Projects;

namespace Portal.BLL.Services
{
    public interface IExampleProjectService
    {
        Task<IEnumerable<ExampleProject>> GetSequenceAsync(DataQueryOptions filter);
    }
}