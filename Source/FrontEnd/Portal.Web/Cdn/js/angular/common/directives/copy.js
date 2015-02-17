// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.copy', [])
    .directive('cbCopy', function() {
            return {
                restrict: 'A',
                link: function(scope, element) {
                    element.bind('click', function() {
                        var content = element.text();
                        if (content) {
                            content = content.trim();
                            window.prompt("Copy to clipboard: Ctrl+C, Enter", content);
                        }
                    });
                },
            };
        }
    )