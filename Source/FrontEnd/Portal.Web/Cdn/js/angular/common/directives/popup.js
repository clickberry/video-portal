// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.popup', [])
    .directive('cbPopup', [
        'jQuery', '$document', function ($, $document) {

            var handlers = [];

            // Execute closing on Esc key press
            $document.keyup(function (e) {

                if (e.which == 27 && handlers.length > 0) {
                    handlers[handlers.length - 1](false);
                }

            });

            function link(scope, element, attrs) {

                var closeExpr = attrs.cbPopupClose;
                var showExpr = attrs.ngShow;
                var visible = false;

                // Watching for changes
                scope.$watch(showExpr, function(value) {

                    if (visible == value) {
                        return;
                    }

                    visible = value;

                    onVisibilityChanged();

                });

                // Handle close events
                function closeEventHandler(destroy) {

                    if (!visible) {
                        return;
                    }

                    visible = false;

                    onVisibilityChanged();

                    // Don't notify about destroy
                    if (destroy) {
                        return;
                    }

                    // Execute callback if it defined
                    if (typeof scope[closeExpr] == "function") {

                        scope.$apply(function() {
                            scope[closeExpr]();
                        });

                    }
                }

                // EVENT HANDLERS

                // Execute closing on clicks in empty area
                element.on("click", function() {
                    closeEventHandler(false);
                });

                // Handle clicks within children
                element.children().on("click", function(e) {
                    e.stopPropagation();
                });

                // Directive destructor
                scope.$on('$destroy', function() {
                    closeEventHandler(true);
                });

                // Handle visibility changes
                function onVisibilityChanged() {

                    if (visible) handlers.push(closeEventHandler);
                    else handlers.pop();

                    $document.find("body")
                        .toggleClass("popup-opened", handlers.length > 0)
                        .css({ "overflow": handlers.length > 0 ? "hidden" : "" });

                }

            };

            return {
                restrict: 'EA',
                link: link
            };
        }
    ]);