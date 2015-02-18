// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using FileInformation;
using Portal.DTO.Common;

namespace Asp.Infrastructure.Validation
{
    /// <summary>
    ///     Determines whether passed file contains a supported file format.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class ImageFileAttribute : ValidationAttribute
    {
        private readonly List<ImageFormat> _formats;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public ImageFileAttribute()
        {
            _formats = new List<ImageFormat>
            {
                ImageFormat.Bmp,
                ImageFormat.Png,
                ImageFormat.Jpeg,
                ImageFormat.Gif
            };
        }


        /// <summary>
        ///     Validates.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var fileEntity = value as FileEntity;
            if (fileEntity == null)
            {
                return ValidationResult.Success;
            }

            var fileInformation = new ImageFileInformation();

            try
            {
                fileInformation.Read(fileEntity.Uri);
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

            if (!_formats.Contains(fileInformation.Format))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}