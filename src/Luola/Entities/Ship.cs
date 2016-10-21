#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using Luola.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola.Entities
{
    public class Ship : Entity
    {
        private bool _colliding;
        private float _rotation;

        public Ship(Game game, string name, Color color, Vector2 position) : base(game)
        {
            Name = name;
            Position = position;
            PreviousPosition = Position;
            Velocity = new Vector2(0, 0);
            Speed = 10f;
            _rotation = 0f;
            IsAlive = true;
            Color = color;
            Health = 100;
            MaxHealth = Health;
            CollideOutside = true;
            Texture = game.Content.Load<Texture2D>("ship");

            PrimaryWeapon = LuolaGame.WeaponManager.InitWeapon("pellet", this);
        }

        public string Name { get; }

        private Texture2D Texture { get; }

        private Weapon PrimaryWeapon { get; set; }
        private Weapon SecondaryWeapon { get; set; }

        public Color Color { get; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }

        public float Speed { get; set; }
        public Vector2 Velocity { get; private set; }
        public Vector2 Direction => Vector2.Transform(-Vector2.UnitY, Matrix.CreateRotationZ(_rotation));
        public Vector2 FrontPosition => Position + 16*Direction;
        public Vector2 BackPosition => Position - 16*Direction;

        public Rectangle Rectangle
        {
            get { return new Rectangle(Position.ToPoint(), new Point(32, 32)); }
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsAlive)
                return;

            PreviousPosition = Position;
            Velocity += Vector2.UnitY/4;
            if (!_colliding)
                Position += Speed*Velocity*(float) gameTime.ElapsedGameTime.TotalSeconds;
            _colliding = false;

            Velocity *= 0.95f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, null, Color, _rotation, new Vector2(16f, 16f), SpriteEffects.None, 0.5f);
            spriteBatch.Draw(LuolaGame.BaseTexture, new Rectangle((Position - Vector2.One).ToPoint(), new Point(2, 2)),
                null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.51f);
        }

        public void SetWantedMove(float thrust, float rotation)
        {
            _rotation += rotation/30;
            Velocity += Vector2.Transform(new Vector2(0, -thrust), Matrix.CreateRotationZ(_rotation));
        }

        public void ApplyVelocity(Vector2 vector)
        {
            Velocity += vector;
        }

        public void ActivatePrimaryWeapon(GameTime gameTime)
        {
            ActivateWeapon(gameTime, PrimaryWeapon);
        }

        public void ActivateSecondaryWeapon(GameTime gameTime)
        {
            ActivateWeapon(gameTime, SecondaryWeapon);
        }

        private void ActivateWeapon(GameTime gameTime, Weapon weapon)
        {
            if (!IsAlive || (weapon == null) || !weapon.CanActivate(gameTime))
                return;

            weapon.Activate(gameTime);
        }

        public void TakeDamage(GameTime gameTime, int damage)
        {
            Health -= damage;
            if (Health <= 0)
                Kill(gameTime);
        }

        public override void Kill(GameTime gameTime)
        {
            base.Kill(gameTime);
            Match.AddDestruction(new Destruction(LuolaGame.DestructionTypeManager.GetDestructionType(64), Position, 20,
                this, false));
        }

        public override void Collided(GameTime gameTime, float x, float y)
        {
            if (Velocity.LengthSquared() > 50)
                TakeDamage(gameTime, (int) Velocity.LengthSquared()/50);
            Velocity = new Vector2(0, 0);
            _colliding = true;
            Position = new Vector2(x, y);
        }

        public void CheckCollisionWithEntity(GameTime gameTime, Entity entity)
        {
            var projectile = entity as Projectile;
            if (projectile != null)
                CheckCollisionsWithProjectile(gameTime, projectile);

            var pickup = entity as Pickup;
            if ((pickup != null) && pickup.Active)
                CheckCollisionsWithPickup(gameTime, pickup);
        }

        private void CheckCollisionsWithProjectile(GameTime gameTime, Projectile projectile)
        {
            if (projectile.Owner == this)
                if (projectile.CreatedAt + projectile.GraceTime > (float) gameTime.TotalGameTime.TotalSeconds)
                    return;

            var dist = (projectile.Position - Position).LengthSquared();
            if (dist > 240)
                return;

            projectile.Kill(gameTime);
            TakeDamage(gameTime, projectile.Damage);
        }

        private void CheckCollisionsWithPickup(GameTime gameTime, Pickup pickup)
        {
            var dist = (pickup.Position - Position).LengthSquared();
            if (dist > 240)
                return;

            var weapon = pickup.CreateWeaponFor(this);
            if (weapon != null)
                if (weapon.Primary)
                    PrimaryWeapon = weapon;
                else
                    SecondaryWeapon = weapon;
            pickup.Kill(gameTime);
        }
    }
}