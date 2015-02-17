// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.select', []).directive('cbSelect', [
    function () {

        return {
            restrict: 'E',
            require: 'ngModel',
            scope: {
                placeholder: '@',
                model: '=ngModel',
                options: '=ngOptions'
            },
            templateUrl: 'select.html',
            controller: ['$scope', function($scope) {


                // PROPERTIES


                $scope.label = $scope.placeholder;

                // setting current option if model specified or changed
                $scope.$watch(modelTrigger, function (value) {

                    initLabel(value);

                });

                $scope.optionsVisible = false;



                // METHODS

                $scope.toggleOptions = function () {

                    $scope.optionsVisible = !$scope.optionsVisible;

                };

                $scope.selectItem = function(key, value) {
                    $scope.model = value;
                    $scope.label = key;

                    $scope.optionsVisible = false;
                };

                function initLabel(initValue) {
                    
                    angular.forEach($scope.options, function (value, key) {

                        if (value == initValue) {
                            $scope.label = key;
                        }

                    });

                }

                function modelTrigger() {
                    return $scope.model;
                }

            }]
        };
    }
])