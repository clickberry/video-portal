// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Configuration.Azure.Concrete
{
    /// <summary>
    ///     Configures temp paths.
    /// </summary>
    public sealed class TempPathInitializer : IInitializable
    {
        public void Initialize()
        {
            string localResourcePath = RoleEnvironment.GetLocalResource("LocalStorage").RootPath;
            Trace.TraceInformation("Temp folder: {0}", localResourcePath);

            Environment.SetEnvironmentVariable("TMP", localResourcePath);
            Environment.SetEnvironmentVariable("TEMP", localResourcePath);
        }
    }
}