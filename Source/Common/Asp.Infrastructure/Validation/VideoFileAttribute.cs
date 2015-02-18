// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using FileInformation;
using Portal.DTO.Common;

namespace Asp.Infrastructure.Validation
{
    /// <summary>
    ///     Determines whether passed file contains a video stream.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class VideoFileAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Validates.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var fileEntity = (FileEntity)value;
            if (fileEntity == null)
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            var videoInformation = new VideoFileInformation();

            try
            {
                videoInformation.Read(fileEntity.Uri);
            }
            catch (ArgumentNullException)
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to receive file information: {0}", e);
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            if (videoInformation.Format == VideoFormat.Undefied)
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            return null;
        }
    }
}