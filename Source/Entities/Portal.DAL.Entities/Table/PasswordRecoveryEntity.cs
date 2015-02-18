// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using MongoRepository;

namespace Portal.DAL.Entities.Table
{
    [CollectionName("PasswordRecovery")]
    public sealed class PasswordRecoveryEntity : Entity
    {
        public string UserId { get; set; }

        public string LinkData { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public DateTime Expires { get; set; }

        public bool IsConfirmed { get; set; }

        public string Email { get; set; }
    }
}