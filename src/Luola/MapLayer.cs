#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public class MapLayer
    {
        public string Image { get; set; }
        public string Type { get; set; }
        public float Parallax { get; set; }

        public Texture2D Texture { get; set; }
        public Color[] Colors { get; set; }
        public bool[,] Collisions { get; set; }

        public int Width => Texture.Width;
        public int Height => Texture.Height;

        public void CalculateCollisions()
        {
            if (Type == "detail")
                return;

            Colors = new Color[Width*Height];
            Collisions = new bool[Width, Height];

            Texture.GetData(Colors);

            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                {
                    var solid = Colors[x + Width*y] != Color.Transparent;
                    Collisions[x, y] = solid;
                }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            spriteBatch.Draw(Texture, cameraPos*-Parallax, null, Color.White, 0f, Vector2.Zero, 1f,
                SpriteEffects.None, 0.01f);
        }

        public void UpdateTextureFromColors()
        {
            Texture.SetData(Colors);
        }

        public void Dispose()
        {
            Texture.Dispose();
        }
    }
}