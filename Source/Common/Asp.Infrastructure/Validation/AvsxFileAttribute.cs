// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Text;
using Portal.DTO.Common;

namespace Asp.Infrastructure.Validation
{
    /// <summary>
    ///     Determines whether file has avsx content.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class AvsxFileAttribute : ValidationAttribute
    {
        private const int HeaderLength = 1024;

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var fileEntity = value as FileEntity;
            if (fileEntity == null)
            {
                return ValidationResult.Success;
            }

            try
            {
                using (FileStream fileStream = File.OpenRead(fileEntity.Uri))
                {
                    if (fileStream.Length > 0)
                    {
                        var buffer = new byte[HeaderLength];
                        int readed = fileStream.Read(buffer, 0, buffer.Length);
                        string fileHeader = Encoding.UTF8.GetString(buffer, 0, readed);

                        if (fileHeader.ToLowerInvariant().Contains(@"<avsx"))
                        {
                            return ValidationResult.Success;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Failed to receive file information: {0}", e);
            }

            return new ValidationResult(FormatErrorMessage(context.DisplayName));
        }
    }
}