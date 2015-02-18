// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Timers;
using Portal.BLL.Statistics.Generator;
using Portal.BLL.Statistics.Helper;

namespace Configuration.ReportGenerator
{
    public sealed class ReportGeneratorInitializer : IInitializable, IDisposable
    {
        private readonly IIntervalHelper _intervalHelper;
        private readonly IReportGenerator _reportGenerator;
        private readonly Timer _timer;
        private bool _isFirst;

        public ReportGeneratorInitializer(IIntervalHelper intervalHelper, IReportGenerator reportGenerator)
        {
            _intervalHelper = intervalHelper;
            _reportGenerator = reportGenerator;

            _timer = new Timer { Interval = 1000 };
            _isFirst = true;
        }

        public void Dispose()
        {
            _timer.Close();
        }

        public void Initialize()
        {
            _timer.Elapsed += async (sender, e) =>
            {
                _timer.Stop();
                DateTime curDateTime = DateTime.UtcNow;
                DateTime dateTime = _intervalHelper.GetLastDate(1, curDateTime);
                if (_isFirst)
                {
                    _isFirst = false;
                    try
                    {
                        await _reportGenerator.GenerateIfNotExist(dateTime);
                    }
                    catch (Exception exception)
                    {
                        Trace.TraceError("Failed to save initial report: {0}", exception);
                    }
                }
                else
                {
                    try
                    {
                        await _reportGenerator.Generate(dateTime);
                    }
                    catch (Exception exception)
                    {
                        Trace.TraceError("Failed to save report: {0}", exception);
                    }
                }
                _timer.Interval = _intervalHelper.GetMillisecondsToEndDay(new TimeSpan(0, 1, 1, 0), DateTime.UtcNow);
                _timer.Start();
            };
            _timer.Start();
        }
    }
}