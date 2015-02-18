// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

namespace Portal.DTO.User
{
    /// <summary>
    ///     Register user data.
    /// </summary>
    public class CreateUserData : Profile
    {
        public virtual string Password { get; set; }

        public virtual string ConfirmPassword { get; set; }
    }
}