// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.DTO.Common;

namespace Portal.DTO.Files
{
    /// <summary>
    ///     File data.
    /// </summary>
    public class UploadFileData
    {
        /// <summary>
        ///     File name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     File.
        /// </summary>
        public virtual FileEntity File { get; set; }
    }
}