using System;
using Luola.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola.Entities
{
    public class Pickup : Entity
    {
        private readonly Vector2 _originalPosition;
        private readonly float _respawnTime;
        private readonly Texture2D _texture;
        private float _pickupTime;
        private string _weaponName;
        public bool Active;

        public Pickup(Game game, Vector2 position) : base(game)
        {
            Position = position;
            _originalPosition = position;
            _texture = game.Content.Load<Texture2D>("pickup");
            _respawnTime = 20f;
            Activate();
        }

        private void Activate()
        {
            _pickupTime = -1;
            Active = true;
            _weaponName = LuolaGame.WeaponManager.RandomWeaponName();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Active)
                if (_pickupTime == -1)
                {
                    _pickupTime = (float) gameTime.TotalGameTime.TotalSeconds;
                }
                else
                {
                    if (_pickupTime + _respawnTime < (float) gameTime.TotalGameTime.TotalSeconds)
                        Activate();
                }

            if (Active)
                Position = _originalPosition + Vector2.UnitY*(float) Math.Sin(gameTime.TotalGameTime.TotalSeconds*2)*10;
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

        public Weapon CreateWeaponFor(Ship ship)
        {
            if (!Active)
                return null;

            return LuolaGame.WeaponManager.InitWeapon(_weaponName, ship);
        }

        public override void Kill()
        {
            Active = false;
        }
    }
}