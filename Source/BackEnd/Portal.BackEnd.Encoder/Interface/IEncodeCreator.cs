// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Portal.BackEnd.Encoder.Interface
{
    public interface IEncodeCreator
    {
        IFfmpegParser CreateFfmpegParser();

        IEncodeStringFactory CreateEncodeStringFactory();

        IEncodeStringBuilder CreateEncodeStringBuilder(ITempFileManager tempFileManager, IEncodeStringFactory encodeStringFactory);

        IDataReceivedHandler CreateDataReceivedHandler(IFfmpegParser parser);
    }
}