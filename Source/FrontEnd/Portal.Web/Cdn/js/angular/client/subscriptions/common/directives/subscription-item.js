// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('directives.clients.subscriptions', [
    'resources.settings',
    'directives.confirm-click'
]).directive('cbSubscriptionItem', [
    'settings', function (settings) {

        return {
            restrict: 'E',
            require: 'ngModel',
            replace: true,
            scope: {
                model: '=ngModel',
                onDelete: '=',
                onStats: '='
            },
            templateUrl: 'client.subscription.item.html',
            controller: [
                '$scope', function ($scope) {


                    $scope.players = [
                        { key: "jw5", value: "JWPlayer5" },
                        { key: "jw6", value: "JWPlayer6" }
                    ];

                    var domain = settings.get('linkTrackerDomain');

                    var subdomainRegexp = /^([^.\s]+)\.clbr\.tv$/;
                    var matches = domain.match(subdomainRegexp);

                    var linkTrackerSetting = matches ? matches[1] : '';

                    $scope.embed = {
                        linkTrackerSetting: linkTrackerSetting,
                        jwFlashPlayerUrl: settings.get('jwFlashPlayerUrl')
                    };
                }
            ],
            link:
                function (scope, element, attributes) {

                }

        };
    }
]).directive('cbChosenSelect', function () {

    return {
        restrict: 'C',
        require: 'ngModel',
        scope: {
            model: '=ngModel',
            players: '=players'
        },
      //  template: '<option ng-selected="$index == 0" ng-repeat="p in players" value="{{p.key}}">{{p.value}}</option>',
        link: function (scope, element) {

               $(element).selectize({
                   options: scope.players,
                   allowEmptyOption: false,
                   valueField: 'key',
                   labelField: 'value',
                   onInitialize: function () {
                   }
               });

        }

    }


});