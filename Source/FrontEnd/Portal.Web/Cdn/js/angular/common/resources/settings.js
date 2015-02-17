// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('resources.settings', []).factory('settings', function () {

    var settings = {
        id: null,
        siteUrl: null,
        linkTrackerDomain: null,
        jwFlashPlayerUrl: null,
        youtubeHtml5PlayerUrl: null,
        jiraIssueCollector: null,
        stripePublicKey: null,
        playerUrl: null,
        videoUrl: "/video",
        userVideosUrl: "/admin/uservideos?id=",
        pageSize: 20,
        resourceStates: [
            { name: "Available", value: 0 },
            { name: "Blocked", value: 1 },
            { name: "Deleted", value: 2 }
        ],
    };

    return {
        set: function(name, value) {

            if (!settings.hasOwnProperty(name)) {
                throw new Error("Unknown setting name: " + name);
            }

            settings[name] = value;
        },
        get: function(name) {

            if (!settings.hasOwnProperty(name)) {
                throw new Error("Unknown setting name: " + name);
            }

            return settings[name];
        }

    };

});