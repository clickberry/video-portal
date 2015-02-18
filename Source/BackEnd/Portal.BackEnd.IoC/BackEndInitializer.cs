// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using RestSharp;
using SimpleInjector;

namespace Portal.BackEnd.IoC
{
    public class BackEndInitializer : BackEndInitializerBase
    {
        public override void Initialize(Container container)
        {
            Initialize(container, new RestClient { FollowRedirects = false, Timeout = 100000 });
        }
    }
}