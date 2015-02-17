angular.module("ngClickberry.account", [
        'ngClickberry.account.recovery',
        'ui.router'
    ])

// Routes
    .config([
        "$stateProvider", function($stateProvider) {
            $stateProvider
                .state('portal.account', {
                    "abstract": true,
                    template: '<div ui-view></div>'
                });
        }
    ]);