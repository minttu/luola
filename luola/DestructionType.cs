﻿using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace luola
{
    public class DestructionType
    {
        public Texture2D ExplosionTexture;
        public bool[,] EraseData;
        public int Size;

        public DestructionType(Game game, int size)
        {
            Size = size;
            EraseData = new bool[size, size];
            float diam = size/2f;
            float diamsq = diam*diam;
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var pos = new Vector2(x - diam, y - diam);
                    var solid = pos.LengthSquared() <= diamsq;
                    EraseData[y, x] = solid;
                }
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