// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.progressbar', []).directive('cbProgressbar', [
    'jQuery', function (jQuery) {

        var $ = jQuery;
        if (!$.fn.progressbar) {
            throw new Error("jQuery UI progressbar required");
        }

        return {
            restrict: 'EA',
            scope: {
                value: '=progressbarValue',
                maxValue: '=progressbarMaxValue',
                criticalValue: '=progressbarCriticalValue',
                criticalClass: '=progressbarCriticalClass',
            },
            link: function(scope, element) {
                
                scope.$watch('value', function (newValue) {
                    if (newValue) {

                        $(element).progressbar({
                            max: scope.maxValue,
                            value: scope.value
                        });

                    }
                });


                if (scope.criticalValue && scope.criticalClass && scope.value >= scope.criticalValue) {
                    element.addClass(scope.criticalClass);
                }
            }
        };
    }
])