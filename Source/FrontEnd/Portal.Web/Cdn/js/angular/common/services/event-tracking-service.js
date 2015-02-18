// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('services.eventTracking', [])
    .factory('eventTrackingService', ['$window',
         function ($window) {

            var service = {};

            service.track = function(category, action, label, intValue) {

                if (!$window.ga) {
                    return;
                }

                $window.ga('send', 'event', category, action, label, intValue);  // value is a number.

            };
            return service;
        }
    ]);