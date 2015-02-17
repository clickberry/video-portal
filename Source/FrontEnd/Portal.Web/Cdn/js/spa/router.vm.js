// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Defines view model for a router
define("router.vm", ["knockout"],
    function(ko) {
        return function(vm, path, elementId, menuId) {
            var instance = null;

            // lazy initialization
            this.model = function() {
                if (!instance) {
                    instance = new vm();
                    ko.applyBindings(instance, document.getElementById(elementId));
                }

                return instance;
            };

            this.element = {
                id: elementId,
                menuId: menuId,
                path: path
            };
        };
    });