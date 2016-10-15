#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public class DestructionType
    {
        public readonly bool[,] EraseData;
        public readonly Texture2D ExplosionTexture;
        public readonly int Size;

        public DestructionType(Game game, int size)
        {
            Size = size;
            EraseData = new bool[size, size];
            var diam = size/2f;
            var diamsq = diam*diam;
            for (var y = 0; y < size; y++)
                for (var x = 0; x < size; x++)
                {
                    var pos = new Vector2(x - diam, y - diam);
                    var solid = pos.LengthSquared() <= diamsq;
                    EraseData[y, x] = solid;
                }

            try
            {
                ExplosionTexture = game.Content.Load<Texture2D>("explosions/" + size);
            }
            catch (ContentLoadException)
            {
                ExplosionTexture = null;
            }
        }
    }
}