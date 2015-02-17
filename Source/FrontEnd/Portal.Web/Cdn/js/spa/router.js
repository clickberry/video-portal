// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// SPA router
define("router", ["sammy", "presenter", "routes"],
    function(sammy, presenter, routes) {

        // Current view model
        var currentModel = null;

        // Handles page changes
        function changePage(context, vm) {

            if (currentModel && currentModel.deactivate) {
                currentModel.deactivate();
            }

            currentModel = vm.model();

            if (currentModel.activate) {
                currentModel.activate(context.params);
            }

            presenter.changePage(vm.element);
        }

        // Initialize hash based routing
        var app = sammy(function() {
            var self = this;
            routes.forEach(function(route) {
                self.get(route.element.path, function(context) {
                    changePage(context, route);
                });
            });
        });

        return {
            run: function() {
                app.run();
            }
        };
    }
);