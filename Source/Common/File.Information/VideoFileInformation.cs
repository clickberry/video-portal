// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FileInformation
{
    /// <summary>
    ///     Receive video file information.
    /// </summary>
    public sealed class VideoFileInformation : FileInformationBase
    {
        private readonly Dictionary<VideoFormat, Dictionary<VideoFormatProfile, Regex>> _fileformats;

        public VideoFileInformation()
        {
            //
            // Based on http://www.garykessler.net/library/file_sigs.html
            //

            _fileformats = new Dictionary<VideoFormat, Dictionary<VideoFormatProfile, Regex>>
            {
                {
                    VideoFormat.Mpeg4, new Dictionary<VideoFormatProfile, Regex>
                    {
                        { VideoFormatProfile.Mpeg3Gp, new Regex("^000000[0-9a-f]{2}66747970336770", RegexOptions.Compiled) },
                        { VideoFormatProfile.Mpeg4Base, new Regex("^000000[0-9a-f]{2}6674797069736f6d", RegexOptions.Compiled) },
                        { VideoFormatProfile.Mpeg4QuickTime, new Regex("^000000[0-9a-f]{2}6674797071742020", RegexOptions.Compiled) },
                        { VideoFormatProfile.Mpeg4Version2, new Regex("^000000[0-9a-f]{2}667479706d703432", RegexOptions.Compiled) },
                        { VideoFormatProfile.Mpeg4Flash, new Regex("^000000[0-9a-f]{2}6674797066347620", RegexOptions.Compiled) },
                        { VideoFormatProfile.Mpeg4SonyPsp, new Regex("^000000[0-9a-f]{2}667479704d534e56", RegexOptions.Compiled) },
                    }
                },
                {
                    VideoFormat.Mpeg, new Dictionary<VideoFormatProfile, Regex>
                    {
                        { VideoFormatProfile.Default, new Regex("^000001b[0-9a-f]", RegexOptions.Compiled) },
                    }
                },
                {
                    VideoFormat.Matroska, new Dictionary<VideoFormatProfile, Regex>
                    {
                        { VideoFormatProfile.Default, new Regex("^1a45dfa3934282886d6174726f736b61", RegexOptions.Compiled) },
                    }
                },
                {
                    VideoFormat.Wmv, new Dictionary<VideoFormatProfile, Regex>
                    {
                        { VideoFormatProfile.Default, new Regex("^3026b2758e66cf11a6d900aa0062ce6c", RegexOptions.Compiled) },
                    }
                },
                {
                    VideoFormat.Flash, new Dictionary<VideoFormatProfile, Regex>
                    {
                        { VideoFormatProfile.Default, new Regex("^464c5601", RegexOptions.Compiled) },
                    }
                },
                {
                    VideoFormat.Avi, new Dictionary<VideoFormatProfile, Regex>
                    {
                        { VideoFormatProfile.Default, new Regex("^52494646([0-9a-f]{4}|[0-9a-f]{8})415649204c495354", RegexOptions.Compiled) },
                    }
                },
                {
                    VideoFormat.Mov, new Dictionary<VideoFormatProfile, Regex>
                    {
                        {
                            VideoFormatProfile.Default,
                            new Regex("^[0-9]{8}(6d6f6f76|66726565|6d646174|77696465|706e6f74|736b6970)", RegexOptions.Compiled)
                        },
                    }
                },
                {
                    VideoFormat.WebM, new Dictionary<VideoFormatProfile, Regex>
                    {
                        { VideoFormatProfile.Default, new Regex("^1a45dfa3", RegexOptions.Compiled) },
                    }
                },
                {
                    VideoFormat.Ogg, new Dictionary<VideoFormatProfile, Regex>
                    {
                        { VideoFormatProfile.Default, new Regex("^4f67675300020000", RegexOptions.Compiled) }
                    }
                }
            };
        }

        /// <summary>
        ///     Gets a video file format.
        /// </summary>
        public VideoFormat Format { get; private set; }

        /// <summary>
        ///     Gets a video format profile.
        /// </summary>
        public VideoFormatProfile FormatProfile { get; private set; }

        protected override void Update()
        {
            foreach (var fileFormat in _fileformats)
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