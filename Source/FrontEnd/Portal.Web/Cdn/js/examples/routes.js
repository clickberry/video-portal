// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

// Defines router view models and addresses
define("routes", ["router.vm", "vm.example", "vm.examples"],
    function(vm, vmexample, vmexamples) {

        var viewModels = [
            new vm(vmexamples, "/examples", "examplesPage", "examples"),
            new vm(vmexample, "/examples/:videoId", "examplePage", "examples")
        ];

        return viewModels;
    });