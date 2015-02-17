// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Portal.Api.Infrastructure.MediaFormatters.FormData;

namespace Portal.Api.Infrastructure.MediaFormatters.ModelFactories
{
    public interface IModelFactory
    {
        object Create(IFormDataProvider dataProvider);
    }
}