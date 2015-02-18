// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

angular.module('constants.common', [])
    .constant('authEvents', {
        loginSuccess: 'auth-login-success',
        loginFailed: 'auth-login-failed',
        logoutSuccess: 'auth-logout-success',
        sessionTimeout: 'auth-session-timeout',
        notAuthenticated: 'auth-not-authenticated',
        notAuthorized: 'auth-not-authorized',
        authorize: 'auth-authorize'
    })
    .constant('userEvents', {
        likeAdded: 'user-like-added',
        likeDeleted: 'user-like-deleted',
        dislikeAdded: 'user-dislike-added',
        dislikeDeleted: 'user-dislike-deleted',
        videoBecomePublic: 'user-video-become-public',
        videoBecomePrivate: 'user-video-become-private'
    })
    .constant('userRoles', {
        all: '*',
        admin: 'Administrator',
        superAdmin: 'SuperAdministrator',
        user: 'User',
        client: 'Client',
        examplesManager: 'ExamplesManager'
    })
    .constant('portalEvents', {
        categories: {
            portal: 'Portal'
        },
        actions: {
            bannerClick: 'BannerClick'
        },
        labels: {
            frontPage: 'FrontPage',
            videoPage: 'VideoPage'
        }
    });