// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Portal.Api.Infrastructure.MediaFormatters.FormData;
using Portal.Api.Models;

namespace Portal.Api.Infrastructure.MediaFormatters.ModelFactories
{
    public class FileModelFactory : IModelFactory
    {
        public object Create(IFormDataProvider dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            return new FileModel
            {
                Name = dataProvider.GetValue("Name"),
                File = dataProvider.GetFile("File")
            };
        }
    }
}