// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Configuration
{
    public interface IConfigurationProvider
    {
        string Get(string name);

        T Get<T>(string name);

        bool IsEmulated();

        string GetRoleId();
    }
}