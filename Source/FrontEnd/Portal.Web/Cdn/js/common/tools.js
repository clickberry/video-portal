// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

define("tools", ["jquery", "resources"], function($, resources) {

    var defaultErrorHandlers = {
        400: queryModelStateInResponse,
        401: function() {
            window.location = "/";
        },
        403: function() {
            return resources.httpForbidden;
        },
        404: function() {
            return resources.httpNotFound;
        },
        409: function() {
            return resources.httpConflict;
        },
        500: function() {
            return resources.httpInternalServerError;
        }
    };

    function queryModelStateInResponse(response) {
        var jsonValue = $.parseJSON(response.responseText);
        var errorText = "";
        if (jsonValue && jsonValue.modelState) {
            for (var i in jsonValue.modelState) {
                errorText += " ";
                errorText += jsonValue.modelState[i][0];
            }
        }
        return errorText;
    }

    function handleResponseErrors(response, extension) {

        var extended = $.extend(defaultErrorHandlers, extension),
            errorHandler = extended[response.status];

        if (typeof(errorHandler) === "function")
            return errorHandler(response);

        return "";
    }

    function extractQueryParam(name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    function drawButtons(total, actionName) {
        var takeValue = parseInt(extractQueryParam("take"), 10),
            skipValue = parseInt(extractQueryParam("skip"), 10),
            sub = skipValue - takeValue,
            add = skipValue + takeValue,
            orderBy = extractQueryParam("orderby"),
            name = extractQueryParam("name"),
            value = extractQueryParam("value"),
            type = extractQueryParam("type"),
            exctractedOrderDirection = extractQueryParam("orderdirection"),
            orderDirection = (exctractedOrderDirection) ? "&orderdirection=" + exctractedOrderDirection : "";

        $("#loadPrevious")
            .attr('href', actionName +
                "?skip=" + sub +
                "&take=" + takeValue +
                "&orderby=" + orderBy +
                "&name=" + name +
                "&value=" + value +
                "&type=" + type +
                orderDirection)
            .removeClass('disabled');
        $("#loadNext")
            .attr('href', actionName +
                "?skip=" + add +
                "&take=" + takeValue +
                "&orderby=" + orderBy +
                "&name=" + name +
                "&value=" + value +
                "&type=" + type +
                orderDirection)
            .removeClass('disabled');

        if (add >= total) {
            $("#loadNext")
                .addClass("disabled");
        }

        if (sub < 0) {
            $("#loadPrevious")
                .removeAttr("href")
                .addClass("disabled");
        }
    }

    // defaultSorting is array of [int, string] because required by datatables
    function getSortingColumnAndDirection(columns, defaultSorting) {
        var orderby = extractQueryParam("orderby"),
            orderDirection = extractQueryParam("orderdirection") == "desc" ? 'desc' : 'asc',
            columnNo = columns.indexOf(orderby);
        if (columnNo === -1) {
            return defaultSorting;
        }
        return [[columnNo, orderDirection]];
    }

    function isMobileDevice() {
        return /Mobile|iP(hone|od|ad)|Android|BlackBerry|IEMobile|Kindle|NetFront|Silk-Accelerated|(hpw|web)OS|Fennec|Minimo|Opera M(obi|ini)|Blazer|Dolfin|Dolphin|Skyfire|Zune/
            .test(navigator.userAgent);
    }

    return {
        handleResponseErrors: handleResponseErrors,
        extractQueryParam: extractQueryParam,
        drawButtons: drawButtons,
        getSortingColumnAndDirection: getSortingColumnAndDirection,
        isMobileDevice: isMobileDevice
    };
});