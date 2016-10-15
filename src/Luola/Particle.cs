using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public class Particle
    {
        public readonly float CreatedAt;
        public readonly bool IsAnimated;
        public readonly Texture2D Texture;
        public float Duration;
        public bool IsAlive;
        public Vector2 Position;

        public Particle(GameTime gameTime, Vector2 position, Texture2D texture)
        {
            CreatedAt = (float) gameTime.TotalGameTime.TotalSeconds;
            IsAlive = true;
            Duration = 0.3f;
            IsAnimated = texture.Width != texture.Height;
            Position = position;
            Texture = texture;
        }

        public float ExpiresAt => CreatedAt + Duration;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (ExpiresAt < (float) gameTime.TotalGameTime.TotalSeconds)
            {
                IsAlive = false;
                return;
            }

            if (!IsAnimated)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
                return;
            }

            var height = Texture.Height;
            var frames = Texture.Width/height;
            var width = height;
            var timePerFrame = Duration/frames;
            var iter = (int) Math.Floor((gameTime.TotalGameTime.TotalSeconds - CreatedAt)/timePerFrame);
            if (iter >= frames)
            {
                IsAlive = false;
                return;
            }
            spriteBatch.Draw(Texture, new Rectangle(Position.ToPoint(), new Point(width, height)),
                new Rectangle(width*iter, 0, width, height), Color.White);
        }
    }
}