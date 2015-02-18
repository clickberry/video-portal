// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Web.Mvc;

namespace Portal.Web.App_Start
{
    public static class ViewEngineConfig
    {
        public static void Configure()
        {
            // Remove All Engines
            ViewEngines.Engines.Clear();

            // Add Razor Engine
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}