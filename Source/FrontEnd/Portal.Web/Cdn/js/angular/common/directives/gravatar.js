// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.gravatar', ['services.md5']).directive('cbGravatar', [
    'md5Service', function (md5Service) {

        return {
            restrict: 'EA',
            replace: true,
            template: '<img ng-src="{{url}}" />',
            link: function(scope, element, attr) {

                if (!attr.gravatarEmail) {
                    return;
                }


                // PROPERTIES

                scope.url = null;
                scope.$watch(attr.gravatarEmail, function(email) {

                    scope.url = generateUrl(email);

                });


                // METHODS

                function generateUrl(email) {

                    if (!email) {
                        return null;
                    }

                    var url = 'http://www.gravatar.com/avatar/';
                    if (location.protocol === "htts:") {
                        url = 'https://secure.gravatar.com/avatar/';
                    }

                    url += md5Service.calculate(email.toLowerCase());

                    return url;

                }
            }
        };
    }
])