// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Portal.Api.Infrastructure.MediaFormatters.FormData;
using Portal.Api.Models;

namespace Portal.Api.Infrastructure.MediaFormatters.ModelFactories
{
    /// <summary>
    ///     Handles multipart form data body.
    /// </summary>
    public sealed class HttpMultipartForm
    {
        private readonly Dictionary<Type, IModelFactory> _factories;

        public HttpMultipartForm()
        {
            _factories = new Dictionary<Type, IModelFactory>
            {
                { typeof (ProjectAvsxModel), new ProjectAvsxFactory() },
                { typeof (ProjectVideoModel), new ProjectVideoFactory() },
                { typeof (ProjectScreenshotModel), new ProjectScreenshotFactory() },
                { typeof (FileModel), new FileModelFactory() },
                { typeof (ExternalProjectModel), new ExternalProjectFactory() }
            };
        }

        public bool CanRead(Type type)
        {
            return _factories.ContainsKey(type);
        }

        public object GetModel(Type type, IFormDataProvider dataProvider)
        {
            if (!_factories.ContainsKey(type))
            {
                return null;
            }

            return _factories[type].Create(dataProvider);
        }
    }
}