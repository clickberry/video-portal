// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.DTO.User
{
    /// <summary>
    ///     User login model.
    /// </summary>
    public class LoginUserData
    {
        /// <summary>
        ///     Email.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        ///     Password.
        /// </summary>
        public virtual string Password { get; set; }
    }
}