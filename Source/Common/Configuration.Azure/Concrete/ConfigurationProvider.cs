// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Configuration.Azure.Concrete
{
    /// <summary>
    ///     Wrapper around azure inrole environment and asp.net web.config settings.
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        /// <summary>
        ///     Gets a value by name.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <returns>Parameter value.</returns>
        public string Get(string name)
        {
            try
            {
                if (RoleEnvironment.IsAvailable)
                {
                    return RoleEnvironment.GetConfigurationSettingValue(name);
                }

                return ConfigurationManager.AppSettings[name];
            }
            catch (Exception exception)
            {
                throw new ArgumentException(string.Format("Configuration setting was not found for '{0}'", name), exception);
            }
        }

        public T Get<T>(string name)
        {
            return (T)Convert.ChangeType(Get(name), typeof (T));
        }

        public bool IsEmulated()
        {
            if (RoleEnvironment.IsAvailable)
            {
                return RoleEnvironment.IsEmulated;
            }

            return true;
        }

        public string GetRoleId()
        {
            if (RoleEnvironment.IsAvailable)
            {
                return RoleEnvironment.CurrentRoleInstance.Id;
            }

            return string.Empty;
        }
    }
}