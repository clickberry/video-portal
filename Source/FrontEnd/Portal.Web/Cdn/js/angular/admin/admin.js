// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module("ngClickberry.admin", [
        'ngClickberry.admin.projects',
        'ngClickberry.admin.users',
        'ngClickberry.admin.clients',
        'directives.confirm-click',
        'ui.router',
        'ui'
    ])

// Routes
    .config([
        "$stateProvider", function($stateProvider) {
            $stateProvider
                .state('admin', {
                    url: '/admin',
                    templateUrl: 'admin.html'
                });
        }
    ]);