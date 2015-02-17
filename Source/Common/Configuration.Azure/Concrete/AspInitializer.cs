// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.Web.Administration;

namespace Configuration.Azure.Concrete
{
    /// <summary>
    ///     Configures ASP.NET site temp paths.
    /// </summary>
    public sealed class AspInitializer : IInitializable
    {
        private readonly IConfigurationProvider _configurationProvider;

        public AspInitializer(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public void Initialize()
        {
            if (_configurationProvider.IsEmulated())
            {
                return;
            }

            // Instantiate the IIS ServerManager
            using (var serverManager = new ServerManager())
            {
                // Disable idle timeout & set queue length to 5000
                serverManager.ApplicationPoolDefaults.ProcessModel.IdleTimeout = TimeSpan.Zero;
                serverManager.ApplicationPoolDefaults.QueueLength = 5000;

                // Server runtime configuration
                Microsoft.Web.Administration.Configuration applicationConfig = serverManager.GetApplicationHostConfiguration();

                // Server runtime settings
                // http://www.iis.net/configreference/system.webserver/serverruntime
                ConfigurationSection serverRuntimeSection = applicationConfig.GetSection("system.webServer/serverRuntime", "");
                serverRuntimeSection["enabled"] = true;
                serverRuntimeSection["frequentHitThreshold"] = 1;
                serverRuntimeSection["frequentHitTimePeriod"] = TimeSpan.Parse("00:00:10");

                // Compression settings
                // http://www.iis.net/configreference/system.webserver/httpcompression
                ConfigurationSection httpCompressionSection = applicationConfig.GetSection("system.webServer/httpCompression");
                httpCompressionSection["noCompressionForHttp10"] = false;
                httpCompressionSection["noCompressionForProxies"] = false;
                ConfigurationElementCollection dynamicTypesCollection = httpCompressionSection.GetCollection("dynamicTypes");

                ConfigurationElement addElement = dynamicTypesCollection.CreateElement("add");
                addElement["mimeType"] = @"application/json";
                addElement["enabled"] = true;
                try
                {
                    dynamicTypesCollection.Add(addElement);
                }
                catch (COMException)
                {
                    // add json element already exists
                }

                addElement = dynamicTypesCollection.CreateElement("add");
                addElement["mimeType"] = @"application/xml";
                addElement["enabled"] = true;
                try
                {
                    dynamicTypesCollection.Add(addElement);
                }
                catch (COMException)
                {
                    // add xml element already exists
                }

                // Commit the changes
                serverManager.CommitChanges();
            }
        }
    }
}