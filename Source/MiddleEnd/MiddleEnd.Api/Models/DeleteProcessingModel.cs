// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.Domain.ProcessedVideoContext.States;

namespace MiddleEnd.Api.Models
{
    public class DeleteProcessingModel
    {
        public TaskState Result { get; set; }

        public string Message { get; set; }

        public string FileId { get; set; }
    }
}