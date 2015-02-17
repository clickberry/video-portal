// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LinkTracker.Domain;
using LinkTracker.Mappings;
using Portal.BLL.Subscriptions;
using Portal.Common.Helpers;
using Portal.Common.Interceptions;
using Portal.Domain.SubscriptionContext;

namespace LinkTracker.BLL.Concrete.Interceptions
{
    public class UrlTrackingServiceStatInterceptor : IInterceptor
    {
        private readonly string[] _bots = { "facebookexternalhit", "http://www.facebook.com/externalhit_uatext.php", "Twitterbot", "Yahoo! Slurp", "http://help.yahoo.com/help/us/ysearch/slurp" };
        private readonly IMappingEngine _mappingEngine;
        private readonly IUrlTrackingStatService _service;

        public UrlTrackingServiceStatInterceptor(IUrlTrackingStatService service, IMappingEngine mappingEngine)
        {
            _service = service;
            _mappingEngine = mappingEngine;
        }

        public void Intercept(IInvocation invocation)
        {
            // Calls the decorated instance.
            invocation.Proceed();


            if (invocation.GetConcreteMethod().Name ==
                NameOfHelper.MethodName<IUrlTrackingService>(x => x.TrackAsync(null)))
            {
                // TrackAsync called
                var trackTask = (Task<UrlTrackingResult>)invocation.ReturnValue;

                // Filtering bots
                if (HttpContext.Current != null)
                {
                    string userAgent = HttpContext.Current.Request.UserAgent;
                    if (!string.IsNullOrEmpty(userAgent))
                    {
                        if (_bots.Any(bot => userAgent.IndexOf(bot, StringComparison.InvariantCultureIgnoreCase) >= 0))
                        {
                            return;
                        }
                    }
                }

                // Checking result
                trackTask.ContinueWith(async t =>
                {
                    UrlTrackingResult trackingResult = t.Result;
                    if (!trackingResult.IsAccountable)
                    {
                        // skip non tariffing redirects
                        return;
                    }

                    try
                    {
                        DomainTrackingStat trackingStat = _mappingEngine.Map<UrlTrackingResult, DomainTrackingStat>(trackingResult);

                        // counting url
                        await _service.CountAsync(trackingStat);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(string.Format("Could not count external url '{0}': {1}",
                            trackingResult.Redirect, e));
                    }
                });
            }
        }
    }
}