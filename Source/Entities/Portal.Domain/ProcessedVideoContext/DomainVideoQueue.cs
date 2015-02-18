// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Portal.Domain.ProcessedVideoContext
{
    /// <summary>
    ///     Scheduled task entity.
    /// </summary>
    public class DomainVideoQueue
    {
        public string ObjectId { get; set; }

        /// <summary>
        ///     Gets or sets a project identifier.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        ///     Gets or sets an original video file id.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        ///     Gets or sets a user identifier.
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        ///     Tasks.
        /// </summary>
        public List<IProcessedEntity> Tasks { get; set; }
    }
}