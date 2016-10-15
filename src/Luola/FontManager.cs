using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public class FontManager
    {
        private readonly Texture2D _texture;

        public FontManager(Game game)
        {
            _texture = game.Content.Load<Texture2D>("font");
        }

        public Rectangle RectangleFor(char c)
        {
            var val = Convert.ToInt32(c);
            if ((val < 32) || (val > 126))
                val = 63;
            val -= 32;
            var x = val%32;
            var y = val/32;
            return new Rectangle(x*10, y*10, 10, 10);
        }

        public void DrawCharacter(SpriteBatch spriteBatch, char c, Vector2 position, Color color, float size = 1f,
            float rotation = 0f)
        {
            spriteBatch.Draw(_texture, position, RectangleFor(c), color, rotation, Vector2.One*5, Vector2.One*size,
                SpriteEffects.None, 0f);
        }

        public void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color = default(Color),
            float size = 1f, float rotation = 0f, bool floaty = false, float gameTime = 0f)
        {
            var pos = position - Vector2.UnitX*10*size*((float) text.Length/2);
            var i = 0;
            foreach (var c in text.ToUpper())
            {
                var offset = floaty ? Vector2.UnitY*2*size*(float) Math.Sin(gameTime + i) : Vector2.Zero;
                DrawCharacter(spriteBatch, c, pos + offset, color, size, rotation);
                pos += Vector2.UnitX*10*size;
                i += 1;
            }
        }
    }
}