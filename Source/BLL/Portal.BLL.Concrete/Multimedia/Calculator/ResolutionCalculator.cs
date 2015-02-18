// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.BLL.Concrete.Multimedia.Calculator
{
    public class ResolutionCalculator : IResolutionCalculator
    {
        private readonly Dictionary<int, double> _resolutionTable = new Dictionary<int, double>();

        public ResolutionCalculator()
        {
            _resolutionTable.Add(360, 1);
            _resolutionTable.Add(480, 1.334);
            _resolutionTable.Add(720, 2);
            _resolutionTable.Add(1080, 3);
        }

        public List<IVideoSize> Calculate(int videoWidth, int videoHeight)
        {
            var resolutionList = new List<IVideoSize>();
            int newHeight = videoHeight;

            if (newHeight > 1080)
            {
                resolutionList.Add(new VideoSize(videoWidth, newHeight));
                newHeight = 1080;
            }

            Dictionary<int, double> coefList = _resolutionTable.ToDictionary(element => element.Key, element => (double)newHeight/element.Key);
            Dictionary<int, double> absList = coefList.ToDictionary(element => element.Key, element => Math.Abs(1 - element.Value));
            double resValue = absList.Min(d => d.Value);
            int resKey = absList.FirstOrDefault(d => d.Value == resValue).Key;

            double minHeight = newHeight/_resolutionTable[resKey];
            double aspectRatio = (double)videoWidth/videoHeight;

            foreach (var d in _resolutionTable)
            {
                double height = minHeight*d.Value;
                double width = height*aspectRatio;

                var roundHeight = (int)Math.Round(height, 0);
                var roundWidth = (int)Math.Round(width, 0);

                resolutionList.Add(new VideoSize(roundWidth, roundHeight));
                if (d.Key == resKey)
                {
                    break;
                }
            }

            return resolutionList;
        }
    }
}