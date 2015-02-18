// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Xunit;

namespace Portal.Tests.Attributes
{
    /// <summary>
    ///     Skips tests when runned outside cloud environment.
    /// </summary>
    public sealed class IntegrationFactAttribute : FactAttribute
    {
#if CI
        public IntegrationFactAttribute()
        {
            Skip = "Skipped integration test";
        }
#endif
    }
}