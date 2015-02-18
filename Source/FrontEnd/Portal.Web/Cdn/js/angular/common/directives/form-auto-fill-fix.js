// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

// https://medium.com/opinionated-angularjs/7bbf0346acec

angular.module('directives.form-auto-fill-fix', []).directive('formAutofillFix', ['$timeout', function ($timeout) {
    return function (scope, element, attrs) {
        element.prop('method', 'post');
        if (attrs.ngSubmit) {
            $timeout(function () {
                element
                  .unbind('submit')
                  .bind('submit', function (event) {
                    if (!attrs.action) {
                        event.preventDefault();
                    }
                    element
                        .find('input, textarea, select')
                        .trigger('input')
                        .trigger('change')
                        .trigger('keydown');
                      scope.$apply(attrs.ngSubmit);
                  });
            });
        }
    };
}]);