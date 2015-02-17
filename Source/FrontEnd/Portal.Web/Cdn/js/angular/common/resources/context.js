// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('resources.context', []).factory('context', function () {

    var context = {
        user: null,
        lastPaymentSucceed: null
    };

    return context;

});