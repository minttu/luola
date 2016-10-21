#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using Microsoft.Xna.Framework;

namespace Luola
{
    public class MapPointData
    {
        public string Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Point AsPoint => new Point(X, Y);
        public Vector2 AsVector => new Vector2(X, Y);
    }
}