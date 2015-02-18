// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module("ngClickberry.editorspick.video", [
        'ngClickberry.video',
        'directives.popup',
        'directives.player',
        'directives.socialshare',
        'directives.loader',
        'services.watch',
        'services.video.comments',
        'filters',
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", "userRoles", function($stateProvider, userRoles) {
            $stateProvider
                .state('portal.editorspick.video', {
                    url: '/{id}',
                    data: {
                        pageTitle: 'Video on Clickberry Online Storage',
                        authorizedRoles: [userRoles.all]
                    },
                    views: {
                        'video': { // reusing template and controller from ngClickberry.video module
                            controller: 'VideoCtrl',
                            templateUrl: 'portal.video.html'
                        }
                    }
                });
        }
    ]);