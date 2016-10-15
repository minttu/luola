using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public class Particle
    {
        public Particle(GameTime gameTime, Vector2 position, Texture2D texture)
        {
            CreatedAt = (float) gameTime.TotalGameTime.TotalSeconds;
            IsAlive = true;
            Duration = 0.3f;
            IsAnimated = texture.Width != texture.Height;
            Position = position;
            Texture = texture;
        }

        public readonly float CreatedAt;
        public readonly Texture2D Texture;
        public readonly bool IsAnimated;
        public Vector2 Position;
        public bool IsAlive;
        public float Duration;
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

            int height = Texture.Height;
            int frames = Texture.Width/height;
            int width = height;
            float timePerFrame = Duration/(float) frames;
            int iter = (int) Math.Floor((gameTime.TotalGameTime.TotalSeconds - CreatedAt)/timePerFrame);
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