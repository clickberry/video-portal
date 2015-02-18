// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FileInformation
{
    /// <summary>
    ///     Receive image file information.
    /// </summary>
    public sealed class ImageFileInformation : FileInformationBase
    {
        private readonly Dictionary<ImageFormat, Dictionary<ImageFormatProfile, Regex>> _fileFormats;

        public ImageFileInformation()
        {
            //
            // Based on http://www.garykessler.net/library/file_sigs.html
            //

            _fileFormats = new Dictionary<ImageFormat, Dictionary<ImageFormatProfile, Regex>>
            {
                {
                    ImageFormat.Jpeg, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.JpegJfif, new Regex("^ffd8ffe0[0-9a-z]{4}4a46494600") },
                        { ImageFormatProfile.JpegExif, new Regex("^ffd8ffe0[0-9a-z]{4}4578696600") },
                        { ImageFormatProfile.JpegSpiff, new Regex("^ffd8ffe0[0-9a-z]{4}535049464600") },
                        { ImageFormatProfile.Default, new Regex("^ffd8ff") },
                    }
                },
                {
                    ImageFormat.Png, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.Default, new Regex("^89504e470d0a1a0a", RegexOptions.Compiled) }
                    }
                },
                {
                    ImageFormat.Gif, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.Default, new Regex("^47494638(37|39)61", RegexOptions.Compiled) }
                    }
                },
                {
                    ImageFormat.Bmp, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.Default, new Regex("^424d", RegexOptions.Compiled) }
                    }
                },
                {
                    ImageFormat.Jpeg2000, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.Default, new Regex("^0000000c6a5020200d0a") },
                    }
                },
                {
                    ImageFormat.Tga, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.Default, new Regex("^54525545564953494f4e2d5846494c452e00") },
                    }
                },
                {
                    ImageFormat.Tiff, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.TiffMonochrome, new Regex("^0ced") },
                        { ImageFormatProfile.TiffLittleEndian, new Regex("^49492a00") },
                        { ImageFormatProfile.TiffBigEndian, new Regex("^4d4d002a") },
                        { ImageFormatProfile.TiffBigTiff, new Regex("^4d4d002b") },
                        { ImageFormatProfile.Default, new Regex("^492049") },
                    }
                },
                {
                    ImageFormat.Psd, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.Default, new Regex("^38425053") },
                    }
                },
                {
                    ImageFormat.Ico, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.Default, new Regex("^00000100") },
                    }
                },
                {
                    ImageFormat.Cur, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.Default, new Regex("^00000200") },
                    }
                },
                {
                    ImageFormat.Cdr, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.Default, new Regex("^454c49544520436f6d6d616e64657220") },
                    }
                },
                {
                    ImageFormat.Wmf, new Dictionary<ImageFormatProfile, Regex>
                    {
                        { ImageFormatProfile.Default, new Regex("^d7cdc69a") },
                    }
                }
            };
        }

        /// <summary>
        ///     Gets an image format.
        /// </summary>
        public ImageFormatProfile FormatProfile { get; private set; }

        /// <summary>
        ///     Gets an image format profile.
        /// </summary>
        public ImageFormat Format { get; private set; }

        protected override void Update()
        {
            foreach (var fileFormat in _fileFormats)
            {
                foreach (var fileFormatProfile in fileFormat.Value)
                {
                    if (fileFormatProfile.Value.IsMatch(Header))
                    {
                        Format = fileFormat.Key;
                        FormatProfile = fileFormatProfile.Key;

                        return;
                    }
                }
            }
        }
    }
}