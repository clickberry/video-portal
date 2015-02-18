// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('directives.loader', []).directive('cbLoader', [
    function() {

        return {
            restrict: 'EA',
            require: 'ngModel',
            scope: {
                trigger: '=loaderIf'
            },
            template: '<div class="wrapper-loading" ng-show="trigger"><div class="loading"><div class="icon icon-spin"></div> Loading...</div></div>'
        };
    }
])