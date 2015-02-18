// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('resources.session', [])
    .service('session', function() {

        this.create = function(sessionId, userRoles) {
            this.id = sessionId;
            this.userRoles = userRoles;
        };

        this.destroy = function() {
            this.id = null;
            this.userRoles = null;
        };

        return this;
    });