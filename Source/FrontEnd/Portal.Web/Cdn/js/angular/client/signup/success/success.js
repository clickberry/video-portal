// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.client.signup.success", [
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", function($stateProvider) {
            $stateProvider
                .state('client.signup.success', {
                    url: '/signup/success',
                    data: { pageTitle: 'Complete Your Registration' },
                    templateUrl: 'client.signup.success.html'
                });
        }
    ]);