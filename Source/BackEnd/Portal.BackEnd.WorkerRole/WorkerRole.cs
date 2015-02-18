// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Configuration;
using Configuration.Azure.Concrete;
using Microsoft.WindowsAzure.ServiceRuntime;
using Portal.BackEnd.Encoder;
using Portal.BackEnd.IoC;
using SimpleInjector;

namespace Portal.BackEnd.WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private Container _container;

        public override void Run()
        {
            _container.GetInstance<EncodeManager>().Start().Wait();
        }

        public override bool OnStart()
        {
            try
            {
                CreateIoC();

                var initializers = new List<IInitializable>
                {
                    _container.GetInstance<DiagnosticsInitializer>(),
                    _container.GetInstance<TraceListenersInitializer>(),
                    _container.GetInstance<TempPathInitializer>(),
                    _container.GetInstance<BackEndConfigurationInitializer>()
                };

                foreach (IInitializable initializer in initializers)
                {
                    Trace.TraceInformation("Executing '{0}' initializer.", initializer.GetType().Name);
                    initializer.Initialize();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Backed role has failed: {0}", e);
                throw;
            }

            return base.OnStart();
        }

        private void CreateIoC()
        {
            _container = new Container();

            new BackEndInitializer().Initialize(_container);

            try
            {
                _container.Verify();
            }
            catch (Exception exception)
            {
                Trace.TraceError("{0}", exception);
                throw;
            }
        }
    }
}