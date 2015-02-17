// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace LinkTracker.Configuration.Azure
{
    public abstract class AzureConfigurationProviderBase
    {
        protected static string Get(string name)
        {
            try
            {
                return RoleEnvironment.IsAvailable
                    ? RoleEnvironment.GetConfigurationSettingValue(name)
                    : ConfigurationManager.AppSettings[name];
            }
            catch (Exception exception)
            {
                throw new ArgumentException(string.Format("Configuration setting was not found for '{0}'", name),
                    exception);
            }
        }
    }
}