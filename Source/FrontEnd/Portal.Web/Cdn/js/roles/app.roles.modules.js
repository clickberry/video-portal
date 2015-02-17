// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

define("jquery", [], function () { return this.jQuery; });
define("knockout", [], function() { return this.ko; });
define("resources", [], function() { return window.cb.resources; });
define("toastr", [], function() {
    this.toastr.options = { positionClass: 'toast-bottom-right' };
    return this.toastr;
});