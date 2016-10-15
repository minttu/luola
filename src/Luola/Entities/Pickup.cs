using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola.Entities
{
    public class Pickup : Entity
    {
        private readonly Texture2D _texture;
        public bool Active;
        private float _pickupTime;
        private readonly float _respawnTime;

        public Pickup(Game game, Vector2 position) : base(game)
        {
            Position = position;
            _texture = game.Content.Load<Texture2D>("pickup");
            Active = true;
            _respawnTime = 20f;
            _pickupTime = -1;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Active)
            {
                if (_pickupTime == -1)
                {
                    _pickupTime = (float) gameTime.TotalGameTime.TotalSeconds;
                }
                else
                {
                    if (_pickupTime + _respawnTime < (float) gameTime.TotalGameTime.TotalSeconds)
                    {
                        _pickupTime = -1;
                        Active = true;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Active)
                return;

            spriteBatch.Draw(_texture, Position, null, null, Vector2.One*6, 0f, null, Color.White, SpriteEffects.None,
                0f);
        }

        public override void Collided(float x, float y)
        {
        }

        public override void Kill()
        {
            Active = false;
        }
    }
}