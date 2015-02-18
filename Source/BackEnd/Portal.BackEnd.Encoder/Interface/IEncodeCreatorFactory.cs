// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using Portal.Domain.BackendContext.Entity.Base;

namespace Portal.BackEnd.Encoder.Interface
{
    public interface IEncodeCreatorFactory
    {
        IEncodeCreator Create(IEncodeData encodeData);
    }
}