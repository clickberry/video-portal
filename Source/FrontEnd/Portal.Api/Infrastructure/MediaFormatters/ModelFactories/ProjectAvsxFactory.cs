// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.Api.Infrastructure.MediaFormatters.FormData;
using Portal.Api.Models;

namespace Portal.Api.Infrastructure.MediaFormatters.ModelFactories
{
    public class ProjectAvsxFactory : IModelFactory
    {
        public object Create(IFormDataProvider dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            return new ProjectAvsxModel { Avsx = dataProvider.GetFile("Avsx") };
        }
    }
}