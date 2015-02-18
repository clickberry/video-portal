// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace FileInformation
{
    public enum VideoFormatProfile
    {
        Default,

        // ftyp 3gp
        Mpeg3Gp,

        // ftyp isom
        Mpeg4Base,

        // ftyp qt
        Mpeg4QuickTime,

        // ftyp mp42
        Mpeg4Version2,

        // 00 00 01 Bx
        Mpeg,

        // 
        Mpeg4Flash,

        // 
        Mpeg4SonyPsp
    }
}