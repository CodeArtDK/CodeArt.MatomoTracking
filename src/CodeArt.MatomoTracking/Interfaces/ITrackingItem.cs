﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Interfaces
{
    public interface ITrackingItem
    {
        TimeSpan? UserTime { get; }

        Dictionary<int, string> Dimensions { get; }
    }
}
