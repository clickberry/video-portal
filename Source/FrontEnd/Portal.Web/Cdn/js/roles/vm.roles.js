// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

define("vm.roles", ["jquery", "knockout", "datacontext", "tools", "toastr"],
    function init($, ko, dataContext, tools, toastr) {
        return function() {
            var self = this;

            function reportErrors(failArgs) {
                var errors = tools.handleResponseErrors(failArgs);
                if (errors) {
                    toastr.error(errors);
                }
            }

            var rolesDataContext = new dataContext("/api/roles");
            self.roles = ko.observableArray([]);
            self.users = ko.observableArray([]);
            self.addUsers = ko.observableArray([]);
            self.selectedRole = ko.observable();
            self.loadingRoles = ko.observable(true);
            self.loadingUsers = ko.observable(true);
            self.searchingUsers = ko.observable(false);
            self.selectedUser = ko.observable();
            self.foundUsers = ko.observableArray([]);

            // loading role users
            self.changeRole = function(role) {
                self.selectedRole(role.name);
            };

            self.selectedRole.subscribe(function(role) {
                self.loadingUsers(true);
                new dataContext("/api/roles/{0}/users".format(role)).get()
                    .done(function(data) {
                        self.users(data);
                        self.loadingUsers(false);
                    })
                    .fail(reportErrors);
            });

            // add users to role
            self.addUsers = function() {
                var user = self.selectedUser();

                if (!user) {
                    return;
                }

                new dataContext("/api/users/{0}/roles".format(user.userId))
                    .post({ name: self.selectedRole() })
                    .done(function() {
                        self.users.push(user);
                        self.selectedUser(null);
                    })
                    .fail(reportErrors);
            };

            // remove role users
            self.removeUser = function(user) {
                new dataContext("/api/users/{0}/roles".format(user.userId))
                    .remove(null, { name: self.selectedRole() })
                    .done(function(data) {
                        self.users.remove(user);
                    })
                    .fail(reportErrors);
            };

            self.searchUsers = function(query, array) {
                if (!query) {
                    self.selectedUser(null);
                    array([]);
                    return;
                }

                $.getJSON("/api/users?userName={0}".format(query))
                    .done(function(data) {
                        var filteredUsers = [];
                        var users = self.users();
                        var len = data.length > 100 ? 100 : data.length;
                        for (var i = 0; i < len; i++) {
                            var user = data[i];
                            if (users.some(function(item) {
                                return item.userId == user.userId;
                            })) {
                                continue;
                            }

                            filteredUsers.push(user);
                        }
                        array(filteredUsers);
                    })
                    .fail(reportErrors);
            };

            (function loadRoles() {
                rolesDataContext.get()
                    .done(function(data) {
                        self.roles(data);
                    })
                    .fail(reportErrors)
                    .always(function() {
                        self.loadingRoles(false);
                    });
            })();
        };
    });