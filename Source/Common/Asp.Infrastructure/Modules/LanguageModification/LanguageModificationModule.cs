// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Asp.Infrastructure.Modules.LanguageModification
{
    public class LanguageModificationModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }

        public void Dispose()
        {
        }

        private void OnBeginRequest(object sender, EventArgs eventArgs)
        {
            var application = sender as HttpApplication;
            if (application != null)
            {
                HttpResponse response = application.Response;
                HttpRequest request = application.Request;
                //const string localeCookieName = "lang";
                //HttpCookie cookie = request.Cookies[localeCookieName];

                // if user has valid cookie we are setting culture else we are setting cookie
                //if (cookie != null && !(String.IsNullOrWhiteSpace(cookie.Value)))
                //{
                //    SetCulture(cookie.Value);
                //}
                //else
                {
                    string[] preferredLanguages = application.Request.UserLanguages;
                    if (preferredLanguages != null) // Client has send 'accept-language' header
                    {
                        string preferredLanguage = preferredLanguages.FirstOrDefault();
                        if (preferredLanguage != null) //...and specified preferred language. We take only first language ignoring others.
                        {
                            SetCulture(preferredLanguage);
                            //var newCookie = new HttpCookie(localeCookieName, preferredLanguage);
                            //response.Cookies.Add(newCookie);
                        }
                    }
                }
            }
        }

        private void SetCulture(string cultureName)
        {
            try
            {
                var culture = new CultureInfo(cultureName);
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
            }
            catch (CultureNotFoundException)
            {
            }
        }
    }
}