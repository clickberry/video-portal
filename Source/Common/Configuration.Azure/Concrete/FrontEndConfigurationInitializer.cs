// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Ionic.Zip;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Configuration.Azure.Concrete
{
    public sealed class FrontEndConfigurationInitializer : IInitializable
    {
        private const string SitePath = "\\sitesroot\\0";

        private readonly IPortalFrontendSettings _settings;

        public FrontEndConfigurationInitializer(IPortalFrontendSettings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            // updating UI when role started to be sure that all role instances have the same UI
            EnsurePortalUI();

            // updating UI when configuration settings changed
            RoleEnvironment.Changed += (s, e) => EnsurePortalUI();
        }

        private void EnsurePortalUI()
        {
            string fileUri = _settings.PortalUIPackageUri;
            if (String.IsNullOrEmpty(fileUri))
            {
                return;
            }

            string tempPath = Path.GetTempFileName();


            // downloading
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(fileUri, tempPath);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to download UI Portal package: {0}", ex);
                return;
            }


            // extracting
            string rolePath = Environment.GetEnvironmentVariable(EnvironmentVariables.RoleRoot);
            string directoryForExtract = String.Format("{0}{1}", rolePath, SitePath);

            try
            {
                using (var zip = new ZipFile(tempPath))
                {
                    zip.ExtractAll(directoryForExtract, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to unzip UI Portal package to dir {0}: {1}", directoryForExtract, ex);
            }


            // deleting temp file
            try
            {
                File.Delete(tempPath);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to delete temporary UI Portal package: {0}", ex);
            }
        }
    }
}