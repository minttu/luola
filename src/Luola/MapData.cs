﻿#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using System.Collections.Generic;

namespace Luola
{
    public class MapData
    {
        public List<MapLayerData> Layers { get; set; }
        public List<MapPointData> Points { get; set; }
    }
}