// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

define("vm.select", ["jquery", "knockout"], function ($, ko) {
    return function(filteredField) {
        var self = this;
        var map = { "name": { name: "UserName", type: "string" }, "email": { name: "UserIdentifier", type: "string" } },
            options = $.map(map, function(elem, index) {
                return index;
            });

        function mapValue(str) {
            return map[str];
        }

        function mapBack(str) {
            for (i in map) {
                if (map[i].name === str)
                    return i;
            }
        }

        self.options = ko.observableArray(options);
        self.selectedValue = ko.observable(mapBack(filteredField) || "");
        self.mappedValue = ko.computed(function() {
            var tmp = mapValue(self.selectedValue());
            return tmp ? tmp.name : "";
        });
        self.mappedType = ko.computed(function() {
            var tmp = mapValue(self.selectedValue());
            return tmp ? tmp.type : "";
        });
    };
});