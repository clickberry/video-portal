// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Web.Administration;

namespace Configuration.Azure.Concrete
{
    /// <summary>
    ///     Configures IIS logging.
    /// </summary>
    public sealed class IisLoggingInitializer : IInitializable
    {
        private readonly IConfigurationProvider _configurationProvider;

        public IisLoggingInitializer(IConfigurationProvider configurationProvider)
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
                Microsoft.Web.Administration.Configuration applicationConfig = serverManager.GetApplicationHostConfiguration();
                ConfigurationSection httpLoggingSection = applicationConfig.GetSection("system.webServer/httpLogging");

                httpLoggingSection["selectiveLogging"] = @"LogAll";
                httpLoggingSection["dontLog"] = false;

                ConfigurationSection sitesSection = applicationConfig.GetSection("system.applicationHost/sites");
                ConfigurationElement siteElement = sitesSection.GetCollection().Single();
                ConfigurationElement logFileElement = siteElement.GetChildElement("logFile");

                logFileElement["logFormat"] = "W3C";
                logFileElement["period"] = "Hourly";
                logFileElement["enabled"] = true;
                logFileElement["logExtFileFlags"] =
                    "BytesRecv,BytesSent,ClientIP,ComputerName,Cookie,Date,Host,HttpStatus,HttpSubStatus,Method,ProtocolVersion,Referer,ServerIP,ServerPort,SiteName,Time,TimeTaken,UriQuery,UriStem,UserAgent,UserName,Win32Status";

                serverManager.CommitChanges();
            }
        }
    }
}