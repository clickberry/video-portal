// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.DAL.Context
{
    public interface IHttpContextRepository
    {
        string GetUrl();
        string GetUrlReferrer();
        string GetUserHostAddress();
        string GetUserHostName();
        string GetUserAgent();
        string[] GetUserLanguages();
        string GetHttpMethod();
        int GetStatusCode();
    }
}