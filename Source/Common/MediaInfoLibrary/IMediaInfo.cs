// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace MediaInfoLibrary
{
    public interface IMediaInfo
    {
        Task<int> Open(string fileName);

        Task Close();

        Task<string> Get(StreamKind streamKind, int streamNumber, string parameter);
    }
}