// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MediaInfoLibrary
{
    public static class MediaInfoInterop
    {
        public static async Task<IntPtr> NewAsync()
        {
            MediaInfoNew d = MediaInfoInvoke.MediaInfo_New;

            try
            {
                return await Task<IntPtr>.Factory.FromAsync(d.BeginInvoke, d.EndInvoke, null);
            }
            catch (Exception exception)
            {
                throw new MediaInfoException(exception: exception);
            }
        }

        public static async Task DeleteAsync(IntPtr handle)
        {
            MediaInfoDelete d = MediaInfoInvoke.MediaInfo_Delete;

            try
            {
                await Task.Factory.FromAsync((callback, o) => d.BeginInvoke(handle, callback, o), d.EndInvoke, null);
            }
            catch (Exception exception)
            {
                throw new MediaInfoException(exception: exception);
            }
        }

        public static async Task<IntPtr> OpenAsync(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string fileName)
        {
            MediaInfoOpen d = MediaInfoInvoke.MediaInfo_Open;

            try
            {
                return await Task<IntPtr>.Factory.FromAsync((callback, o) => d.BeginInvoke(handle, fileName, callback, o), d.EndInvoke, null);
            }
            catch (Exception exception)
            {
                throw new MediaInfoException(exception: exception);
            }
        }

        public static async Task<IntPtr> OpenAnsiAsync(IntPtr handle, IntPtr fileName)
        {
            MediaInfoAOpen d = MediaInfoInvoke.MediaInfoA_Open;

            try
            {
                return await Task<IntPtr>.Factory.FromAsync((callback, o) => d.BeginInvoke(handle, fileName, callback, o), d.EndInvoke, null);
            }
            catch (Exception exception)
            {
                throw new MediaInfoException(exception: exception);
            }
        }

        public static async Task CloseAsync(IntPtr handle)
        {
            MediaInfoClose d = MediaInfoInvoke.MediaInfo_Close;

            try
            {
                await Task.Factory.FromAsync((callback, o) => d.BeginInvoke(handle, callback, o), d.EndInvoke, null);
            }
            catch (Exception exception)
            {
                throw new MediaInfoException(exception: exception);
            }
        }

        public static async Task<IntPtr> GetAsync(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, [MarshalAs(UnmanagedType.LPWStr)] string parameter, IntPtr kindOfInfo, IntPtr kindOfSearch)
        {
            MediaInfoGet d = MediaInfoInvoke.MediaInfo_Get;

            try
            {
                return await Task<IntPtr>.Factory.FromAsync((callback, o) => d.BeginInvoke(handle, streamKind, streamNumber, parameter, kindOfInfo, kindOfSearch, callback, o), d.EndInvoke, null);
            }
            catch (Exception exception)
            {
                throw new MediaInfoException(exception: exception);
            }
        }

        public static async Task<IntPtr> GetAnsiAsync(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo, IntPtr kindOfSearch)
        {
            MediaInfoAGet d = MediaInfoInvoke.MediaInfoA_Get;

            try
            {
                return await Task<IntPtr>.Factory.FromAsync((callback, o) => d.BeginInvoke(handle, streamKind, streamNumber, parameter, kindOfInfo, kindOfSearch, callback, o), d.EndInvoke, null);
            }
            catch (Exception exception)
            {
                throw new MediaInfoException(exception: exception);
            }
        }
    }
}