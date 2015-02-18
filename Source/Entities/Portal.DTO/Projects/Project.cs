// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.Domain.ProjectContext;

namespace Portal.DTO.Projects
{
    /// <summary>
    ///     Project entity.
    /// </summary>
    public class Project
    {
        /// <summary>
        ///     Gets or sets an identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets a title.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     Gets or sets a description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        ///     Gets or sets a create date.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Gets or sets a public url.
        /// </summary>
        public string PublicUrl { get; set; }

        /// <summary>
        ///     Gets or sets a public visibility.
        /// </summary>
        public virtual ProjectAccess Access { get; set; }

        /// <summary>
        ///     Gets or sets a project type.
        /// </summary>
        public virtual ProjectType ProjectType { get; set; }

        /// <summary>
        ///     Gets or sets a tag type.
        /// </summary>
        public virtual ProjectSubtype ProjectSubtype { get; set; }

        public virtual bool EnableComments { get; set; }
    }
}