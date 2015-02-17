// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.confirm-click', []).directive('cbConfirmClick', function () {
    return {
        restrict: 'A',
        link: function(scope, elt, attrs) {
            elt.bind('click', function() {
                var message = attrs.cbConfirmation || "Are you sure?";
                if (window.confirm(message)) {
                    var action = attrs.cbConfirmClick;
                    if (action)
                        scope.$apply(scope.$eval(action));
                }
            });
        }
    };
})