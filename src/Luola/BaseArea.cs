#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using Luola.Entities;
using Microsoft.Xna.Framework;

namespace luola
{
    public class BaseArea
    {
        public BaseArea(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }

        public Rectangle Rectangle { get; }
        public Ship OccupiedBy { get; set; }
        public bool Occupied => OccupiedBy?.IsAlive == true;
    }
}