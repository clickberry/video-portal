// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MediaInfoLibrary
{
    public sealed class MediaInfo : IMediaInfo
    {
        private IntPtr _handle;
        private bool _mustUseAnsi;

        private MediaInfo()
        {
        }

        public async Task<int> Open(string fileName)
        {
            if (_handle == (IntPtr)0)
            {
                return 0;
            }

            if (_mustUseAnsi)
            {
                IntPtr fileNamePtr = Marshal.StringToHGlobalAnsi(fileName);
                var toReturn = (int)(await MediaInfoInterop.OpenAnsiAsync(_handle, fileNamePtr));
                Marshal.FreeHGlobal(fileNamePtr);
                return toReturn;
            }

            return (int)(await MediaInfoInterop.OpenAsync(_handle, fileName));
        }

        public async Task<string> Get(StreamKind streamKind, int streamNumber, string parameter)
        {
            if (_handle == (IntPtr)0)
            {
                return "Unable to load MediaInfo library";
            }

            if (_mustUseAnsi)
            {
                IntPtr parameterPtr = Marshal.StringToHGlobalAnsi(parameter);
                string toReturn = Marshal.PtrToStringAnsi(
                    await MediaInfoInterop.GetAnsiAsync(
                        _handle,
                        (IntPtr)streamKind,
                        (IntPtr)streamNumber,
                        parameterPtr,
                        (IntPtr)InfoKind.Text,
                        (IntPtr)InfoKind.Name));
                Marshal.FreeHGlobal(parameterPtr);
                return toReturn;
            }

            return Marshal.PtrToStringUni(
                await MediaInfoInterop.GetAsync(
                    _handle,
                    (IntPtr)streamKind,
                    (IntPtr)streamNumber,
                    parameter,
                    (IntPtr)InfoKind.Text,
                    (IntPtr)InfoKind.Name));
        }

        public async Task Close()
        {
            if (_handle == (IntPtr)0)
            {
                return;
            }

            await MediaInfoInterop.CloseAsync(_handle);
            await MediaInfoInterop.DeleteAsync(_handle);
        }

        public static async Task<IMediaInfo> Create()
        {
            var mediaInfo = new MediaInfo();

            try
            {
                mediaInfo._handle = await MediaInfoInterop.NewAsync();
            }
            catch
            {
                mediaInfo._handle = (IntPtr)0;
            }

            mediaInfo._mustUseAnsi = Environment.OSVersion.ToString().IndexOf("Windows", StringComparison.Ordinal) == -1;

            return mediaInfo;
        }
    }
}