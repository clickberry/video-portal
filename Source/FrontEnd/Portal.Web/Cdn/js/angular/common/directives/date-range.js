// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.clients.daterange', [])
    .controller('cbDateRangeCtrl', ['$scope', '$rootScope', '$filter', function ($scope, $rootScope, $filter) {

        $scope.dateFrom = $filter('date')($scope.dateRange.dateFrom, 'dd MMM yyyy');
        $scope.dateTo = $filter('date')($scope.dateRange.dateTo, 'dd MMM yyyy');

        $scope.setDateFrom = function (dateFrom) {
            $scope.dateRange.dateFrom = new Date(dateFrom);
            $scope.setDateRange($scope.dateRange);
        }

        $scope.setDateTo = function (dateTo) {
            $scope.dateRange.dateTo = new Date(dateTo);
            $scope.setDateRange($scope.dateRange);
        }
        

    }])
   .directive('cbDateRange', [
   function () {

        return {
            restrict: 'E',
            require: 'ngModel',
            scope: {
                dateRange: '=ngModel',
                setDateRange: '=ngChange',
                dateFormat: '@dateformat'
            },
            controller: 'cbDateRangeCtrl',
            template: '<div class="daterange-main-container" style="margin: 0 auto;"><input type="text"  class="datePickerFrom" ng-model="dateFrom"  /><span class="delimiter">-</span><input type="text" ng-model="dateTo" class="datePickerTo" /><div style="clear:both;"></div></div>',
            link: function (scope, element, attrs) {

                 $(element).find(".datePickerFrom").datepicker({
                    defaultDate: scope.dateRange.dateFrom,
                    dateFormat: scope.dateFormat,

                    onSelect: function (dateText, inst) {

                        $(element).find(".datePickerTo").datepicker("option", "minDate", dateText);
                        scope.setDateFrom(dateText);
                    }
                });

                 $(element).find(".datePickerTo").datepicker({
                    defaultDate: scope.dateRange.dateTo,
                    dateFormat: scope.dateFormat,

                    onSelect: function (dateText, inst) {
                        $(element).find(".datePickerFrom").datepicker("option", "maxDate", dateText);
                        scope.setDateTo(dateText);
                    }

                });
            } 
        };
    }
])