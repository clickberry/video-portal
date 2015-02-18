// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Portal.BackEnd.Encoder.Exceptions;
using Portal.BackEnd.Encoder.Interface;
using Wrappers;

namespace Portal.BackEnd.Encoder
{
    public class EncodeManager
    {
        private readonly IEncodePipeline _pipeline;
        private readonly TaskStaticWrapper _taskStatic;
        private readonly CancellationTokenSource _token;

        public EncodeManager(IEncodePipeline pipeline, CancellationTokenSource token, TaskStaticWrapper taskStatic)
        {
            _pipeline = pipeline;
            _token = token;
            _taskStatic = taskStatic;
        }

        public Task Start()
        {
            return Task.Factory.StartNew(
                () =>
                {
                    while (!_token.IsCancellationRequested)
                    {
                        try
                        {
                            _pipeline.Run();
                        }
                        catch (NoContentException)
                        {
                            _taskStatic.Delay(5000).Wait();
                        }
                        catch (ResponseWebException ex)
                        {
                            Trace.TraceInformation("WebException was thrown: {0}", ex);
                        }
                        catch (ResponseTimeoutException ex)
                        {
                            Trace.TraceInformation("ResponseTimeoutException was thrown: {0}", ex);
                        }
                        catch (StatusCodeException ex)
                        {
                            Trace.TraceInformation("MiddleEnd return error StatusCode: {0}", ex);
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceError("Failed to process video task: {0}", ex);
                        }
                    }
                });
        }
    }
}