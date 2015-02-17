// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Asp.Infrastructure.Modules.UriModification
{
    public interface IUriModifier
    {
        Uri Process(Uri uri);
    }
}