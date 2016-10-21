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

namespace Luola.Entities
{
    public abstract class Projectile : Entity
    {
        public Projectile(Game game, Vector2 position, Vector2 direction, Entity owner, GameTime time) : base(game)
        {
            Speed = 400f;
            Position = position;
            Direction = direction;
            IsAlive = true;
            Owner = owner;
            Damage = 0;
            DestructionSize = 1;
            Weight = 1;
            GraceTime = 0.5f;
            CollideOutside = false;
            FriendlyFire = true;
            CreatedAt = (float) time.TotalGameTime.TotalSeconds;
        }

        public float CreatedAt { get; }

        public int Weight { get; set; }

        protected int DestructionSize { get; set; }
        public int Damage { get; protected set; }

        protected float Speed
        {
            set { Velocity = Direction*value; }
        }

        protected Vector2 Velocity { get; set; }
        public Entity Owner { get; }
        protected Texture2D Texture { get; set; }
        protected Vector2 Direction { get; set; }
        public bool FriendlyFire { get; protected set; }

        private Rectangle Rectangle
            =>
            new Rectangle((Position - Vector2.One*(Texture.Width/2)).ToPoint(), new Point(Texture.Width, Texture.Height))
            ;

        public float GraceTime { get; protected set; }

        public override void Update(GameTime gameTime)
        {
            PreviousPosition = Position;
            Velocity += Vector2.UnitY/4*Weight;
            Position += Velocity*(float) gameTime.ElapsedGameTime.TotalSeconds;
            Velocity *= 0.999f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }

        public override void Collided(GameTime gameTime, float x, float y)
        {
            Kill(gameTime);
            Match.AddDestruction(new Destruction(LuolaGame.DestructionTypeManager.GetDestructionType(DestructionSize),
                new Vector2(x, y), Damage, Owner, FriendlyFire));
        }
    }
}