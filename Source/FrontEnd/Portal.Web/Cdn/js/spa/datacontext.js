// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

define("datacontext", ["jquery"], function($) {

    return function(resourceUri) {

        if (!resourceUri) {
            throw Error("Invalid resource uri.");
        }

        function getUri(uri, id) {
            return id ? uri + "/" + id : uri;
        }

        this.post = function(requestData) {
            return $.Deferred(function(deferred) {
                $.ajax({
                    type: 'POST',
                    url: getUri(resourceUri),
                    data: requestData,
                    error: function(responseData) {
                        deferred.reject(responseData);
                    },
                    success: function(responseData) {
                        deferred.resolve(responseData);
                    }
                });
            }).promise();
        };

        this.get = function(id) {
            return $.Deferred(function(deferred) {
                $.ajax({
                    url: getUri(resourceUri, id),
                    dataType: 'json',
                    error: function(responseData) {
                        deferred.reject(responseData);
                    },
                    success: function(responseData) {
                        deferred.resolve(responseData);
                    }
                });
            }).promise();
        };

        this.put = function(id, requestData) {
            return $.Deferred(function(deferred) {
                $.ajax({
                    type: 'POST',
                    url: getUri(resourceUri, id),
                    data: requestData,
                    headers: { 'X-HTTP-Method-Override': "PUT" },
                    error: function(responseData) {
                        deferred.reject(responseData);
                    },
                    success: function(responseData) {
                        deferred.resolve(responseData);
                    }
                });
            }).promise();
        };

        this.remove = function(id, requestData) {
            return $.Deferred(function(deferred) {
                $.ajax({
                    type: 'POST',
                    url: getUri(resourceUri, id),
                    data: requestData,
                    headers: { 'X-HTTP-Method-Override': "DELETE" },
                    error: function(responseData) {
                        deferred.reject(responseData);
                    },
                    success: function(responseData) {
                        deferred.resolve(responseData);
                    }
                });
            }).promise();
        };
    };
});