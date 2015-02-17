// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using Stripe;

namespace Portal.Mappers.ValueResolvers
{
    public class StripeEventToIdResolver : ValueResolver<StripeEvent, string>
    {
        protected override string ResolveCore(StripeEvent source)
        {
            if (source == null)
            {
                return null;
            }

            StripeEventData data = source.Data;
            if (data.Object == null || data.Object.id == null)
            {
                return null;
            }

            return data.Object.id.ToString();
        }
    }
}