// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.Api.Infrastructure.MediaFormatters.FormData;
using Portal.Api.Models;
using Portal.Domain.ProjectContext;

namespace Portal.Api.Infrastructure.MediaFormatters.ModelFactories
{
    public class ExternalProjectFactory : IModelFactory
    {
        public object Create(IFormDataProvider dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            var result = new ExternalProjectModel
            {
                Name = dataProvider.GetValue("Name"),
                Description = dataProvider.GetValue("Description"),
                VideoUri = dataProvider.GetValue("VideoUri"),
                ProductName = dataProvider.GetValue("ProductName"),
                Avsx = dataProvider.GetFile("Avsx"),
                Screenshot = dataProvider.GetFile("Screenshot")
            };

            if (dataProvider.Contains("Access"))
            {
                result.Access = dataProvider.GetValue("Access", ProjectAccess.Public);
            }

            if (dataProvider.Contains("EnableComments"))
            {
                result.EnableComments = dataProvider.GetValue("EnableComments", true);
            }

            if (dataProvider.Contains("ProjectType", true))
            {
                result.ProjectType = dataProvider.GetValue("ProjectType", ProjectType.None);
            }

            if (dataProvider.Contains("ProjectSubtype", true))
            {
                result.ProjectSubtype = dataProvider.GetValue("ProjectSubtype", ProjectSubtype.None);
            }

            return result;
        }
    }
}