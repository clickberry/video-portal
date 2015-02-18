// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Microsoft.WindowsAzure.ServiceRuntime;

namespace Configuration.Azure.Concrete
{
    public sealed class BackEndConfigurationInitializer : IInitializable
    {
        public void Initialize()
        {
            // Prevent role recycling on configuration changes
            RoleEnvironment.Changing += (s, e) => { };
        }
    }
}