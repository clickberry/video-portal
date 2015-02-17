// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.WindowsAzure.ServiceRuntime;

namespace Configuration.Azure.Concrete
{
    public sealed class MiddleEndConfigurationInitializer : IInitializable
    {
        public void Initialize()
        {
            // Prevent role recycling on configuration changes
            RoleEnvironment.Changing += (s, e) => { };
        }
    }
}