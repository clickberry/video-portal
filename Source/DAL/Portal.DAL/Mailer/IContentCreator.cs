// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DAL.Mailer
{
    public interface IContentCreator
    {
        string ContentType { get; }
        string CreateContent(string body);
    }
}