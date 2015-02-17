// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

require(["jquery", "knockout", "moment"], function ($, ko, moment) {

    // KO date formatter
    ko.bindingHandlers.dateString = {
        update: function(element, valueAccessor) {
            var value = valueAccessor();
            var valueUnwrapped = ko.utils.unwrapObservable(value);
            var formattedTime = moment.utc(valueUnwrapped).format("DD-MM-YYYY HH:mm:ss");

            $(element).text(formattedTime);
        }
    };

    // KO megaByte formatter
    ko.bindingHandlers.megaByte = {
        update: function(element, valueAccessor) {
            var value = valueAccessor()() / 1048576;
            $(element).text("{0} MB".format(value.toFixed(0)));
        }
    };

    // KO gigaByte formatter
    ko.bindingHandlers.gigaByte = {
        update: function(element, valueAccessor) {
            var value = valueAccessor()() / 1073741824;
            $(element).text("{0} GB".format(value.toFixed(0)));
        }
    };

    // KO progressbar formatter
    ko.bindingHandlers.progressBar = {
        init: function(element, valueAccessor) {
            var value = parseInt(valueAccessor()());
            $(element)
                .progressbar({ value: value })
                .append($("<div class='ui-progressbar-label'></div>"));
        },
        update: function(element, valueAccessor) {
            //assign observable value to progress bar value
            var value = parseInt(valueAccessor()());

            var $element = $(element);
            var $label = $element.progressbar("value", value)
                .children(".ui-progressbar-label")
                .text("{0}%".format(value.toFixed(0)));

            if (value > 85) {
                $element.removeClass("green").addClass("red");
            } else {
                $element.removeClass("red").addClass("green");
            }

            if (value > 55) {
                $label.removeClass("black").addClass("white");
            } else {
                $label.removeClass("white").addClass("black");
            }
        }
    };

    // KO selectmenu formatter
    ko.bindingHandlers.selectMenu = {
        init: function(element, valueAccessor) {
            var selectHandler = function() {
                $(element).trigger("change");
            };

            $(element).selectmenu({ change: selectHandler });
        },
        update: function(element) {
            $(element).selectmenu("refresh");
        }
    };
});