// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace MediaInfoLibrary
{
    public static class MediaInfoInvoke
    {
        [DllImport("MediaInfo.dll")]
        public static extern IntPtr MediaInfo_New();

        [DllImport("MediaInfo.dll")]
        public static extern void MediaInfo_Delete(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        public static extern IntPtr MediaInfo_Open(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string fileName);

        [DllImport("MediaInfo.dll")]
        public static extern IntPtr MediaInfoA_Open(IntPtr handle, IntPtr fileName);

        [DllImport("MediaInfo.dll")]
        public static extern void MediaInfo_Close(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        public static extern IntPtr MediaInfo_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, [MarshalAs(UnmanagedType.LPWStr)] string parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);

        [DllImport("MediaInfo.dll")]
        public static extern IntPtr MediaInfoA_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);
    }
}