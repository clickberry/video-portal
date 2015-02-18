// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

define("vm.videos.select", ["jquery", "knockout", "resources", "tools"], function ($, ko, resources, tools) {
    return function(filteredField, fieldValue, fieldType) {
        var self = this;
        var map = { "title": { name: "Name", type: "string" }, "author": { name: "UserName", type: "string" }, "product": { name: "ProductId", type: "int" } },
            options = $.map(map, function(elem, index) {
                return index;
            }),
            productMap = resources.products,
            productOptions = $.map(productMap, function(elem, index) {
                return index;
            });

        function mapBack(str, obj, prop) {
            for (i in obj) {
                if ((prop ? obj[i][prop] : obj[i]) === str)
                    return i;
            }
        }

        // List of available filters
        self.options = ko.observableArray(options);

        // Selected filter
        self.selectedValue = ko.observable(mapBack(filteredField, map, "name") || "");

        // List of available products
        self.productOptions = ko.observableArray(productOptions);

        // Selected product value
        self.selectedProduct = ko.observable(mapBack(parseInt(fieldValue, 10), productMap));

        self.mappedProduct = ko.computed(function() {
            return productMap[self.selectedProduct()];
        });

        self.mappedValue = ko.computed(function() {
            var tmp = map[self.selectedValue()];
            return tmp ? tmp.name : "";
        });

        self.mappedType = ko.computed(function() {
            var tmp = map[self.selectedValue()];
            return tmp ? tmp.type : "";
        });

        self.productNameSelected = ko.computed(function() {
            return self.selectedValue() === "product";
        });

        self.textBoxValue = ko.observable(filteredField == map["product"].name ? "" : fieldValue);
    };
});