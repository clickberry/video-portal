// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.DTO.Common;

namespace Portal.Api.Infrastructure.MediaFormatters.FormData
{
    public interface IFormDataProvider
    {
        /// <summary>
        ///     Gets a value indicating whether field exists.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="isRequired"></param>
        /// <returns>Whether field exists.</returns>
        bool Contains(string name, bool isRequired = false);

        /// <summary>
        ///     Gets a string value by name.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>Value.</returns>
        string GetValue(string name);

        /// <summary>
        ///     Gets a value by name.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Value.</returns>
        T GetValue<T>(string name, T defaultValue);


        /// <summary>
        ///     Gets a file entity by name.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <returns>File.</returns>
        FileEntity GetFile(string name);
    }
}