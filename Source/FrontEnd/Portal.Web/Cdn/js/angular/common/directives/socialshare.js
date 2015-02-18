// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('directives.socialshare', []).directive('cbSocialShare', [
    'jQuery', function(jQuery) {

        var $ = jQuery;

        if (!$.fn.socialshare) {
            throw new Error("jquery.socialshare.js required");
        }

        return {
            restrict: 'A',
            scope: {
                title: '@shareTitle',
                description: '@shareDescription',
                media: '@shareMedia',
                url: '@shareUrl'
            },
            link: function($scope, element) {

                $(element).socialshare({
                    title: $scope.title,
                    description: $scope.description,
                    media: $scope.media,
                    url: $scope.url
                });

            }
        };
    }
])