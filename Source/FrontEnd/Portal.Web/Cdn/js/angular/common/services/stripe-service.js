// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('services.stripe',
    ['resources.settings'])
    .factory('stripeService', [
        'stripe', 'settings', function (stripe, settings) {

            if (!stripe) {
                throw new Error("Stripe.js required");
            }

            var stripePublicKey = settings.get("stripePublicKey");
            if (!stripePublicKey) {
                throw new Error("Stripe publishable key required");
            }

            // configuring stripe
            stripe.setPublishableKey(stripePublicKey);

            return stripe;
        }
    ]);