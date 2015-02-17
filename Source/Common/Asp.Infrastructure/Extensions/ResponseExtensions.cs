// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;

namespace Asp.Infrastructure.Extensions
{
    public static class ResponseExtensions
    {
        public static void SetLastModifiedDate(this HttpResponseMessage response, DateTime dateTime)
        {
            response.Content.Headers.LastModified = dateTime;
        }
    }
}