// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('directives.config', ['resources.settings']).directive('cbConfig', [
    'settings', function (settings) {

        return {
            restrict: 'EA',
            replace: true,
            scope: {
                id: '@',
                linkTrackerDomain: '@',
                jwFlashPlayerUrl: '@',
                youtubeHtml5PlayerUrl: '@',
                stripePublicKey: '@',
                playerUrl: '@',
                jiraIssueCollector: '@'
            },
            template: '',
            controller: ['$scope', function($scope) {

                settings.set('id', $scope.id);
                settings.set('linkTrackerDomain', $scope.linkTrackerDomain);
                settings.set('jwFlashPlayerUrl', $scope.jwFlashPlayerUrl);
                settings.set('youtubeHtml5PlayerUrl', $scope.youtubeHtml5PlayerUrl);
                settings.set('stripePublicKey', $scope.stripePublicKey);
                settings.set('playerUrl', $scope.playerUrl);
                settings.set('jiraIssueCollector', $scope.jiraIssueCollector);

            }]
        };
    }
])