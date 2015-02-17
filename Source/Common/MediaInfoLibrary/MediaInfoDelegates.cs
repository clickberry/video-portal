// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace MediaInfoLibrary
{
    public delegate IntPtr MediaInfoAGet(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);

    public delegate IntPtr MediaInfoAOpen(IntPtr handle, IntPtr fileName);

    public delegate void MediaInfoClose(IntPtr handle);

    public delegate void MediaInfoDelete(IntPtr handle);

    public delegate IntPtr MediaInfoGet(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, [MarshalAs(UnmanagedType.LPWStr)] string parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);

    public delegate IntPtr MediaInfoNew();

    public delegate IntPtr MediaInfoOpen(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string fileName);
}