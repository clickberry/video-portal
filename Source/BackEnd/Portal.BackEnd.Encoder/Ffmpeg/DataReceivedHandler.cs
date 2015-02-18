// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using Portal.BackEnd.Encoder.Interface;

namespace Portal.BackEnd.Encoder.Ffmpeg
{
    public class DataReceivedHandler : IDataReceivedHandler
    {
        private readonly IFfmpegParser _parser;
        private Action _action = () => { };
        private Action<int> _actionInt = i => { };

        private double _duration;

        public DataReceivedHandler(IFfmpegParser parser)
        {
            _parser = parser;
        }

        public void ProcessData(string data)
        {
            _action();

            if (_duration == 0)
            {
                _duration = _parser.ParseDuration(data);
            }
            else
            {
                double encodeTime = _parser.ParseEncodeTime(data);
                var percent = (int)(encodeTime/_duration*100);
                _actionInt(percent);
            }
        }

        public void Register(Action<int> action)
        {
            _actionInt = action;
        }

        public void Register(Action action)
        {
            _action = action;
        }
    }
}