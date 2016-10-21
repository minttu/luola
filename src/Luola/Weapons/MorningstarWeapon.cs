#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using System;
using Luola.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola.Weapons
{
    public class MorningstarWeapon : Weapon
    {
        public new static string Name = "morningstar";
        public new static float ChanceToAppear = 1f;

        public MorningstarWeapon(Ship owner) : base(owner)
        {
            TimeBetweenActivations = 1.5f;
        }

        public override void Activate(GameTime gameTime)
        {
            base.Activate(gameTime);

            var projectile = new MorningstarProjectile(Owner.Game, Owner.Position, Owner.Direction, Owner, gameTime);
            Owner.Match.AddEntity(projectile);

            Owner.ApplyVelocity((Owner.Position - Owner.FrontPosition)/2);
        }
    }

    internal class MorningstarProjectile : Projectile
    {
        public MorningstarProjectile(LuolaGame game, Vector2 position, Vector2 direction, Entity owner, GameTime time)
            : base(game, position, direction, owner, time)
        {
            Damage = 20;
            DestructionSize = 32;
            Speed = 250f;
            Weight = 10;
            Texture = game.Content.Load<Texture2D>("weapons/morningstar_projectile.png");
        }

        public override void Kill(GameTime gameTime)
        {
            const int projectiles = 32;
            for (var i = 0; i < projectiles; i++)
            {
                var dir = Vector2.Transform(-Vector2.UnitY, Matrix.CreateRotationZ((float) (Math.PI*2/projectiles*i)));
                var projectile = new MorningstarSubProjectile(Owner.Game, Position, dir, Owner, gameTime);
                Owner.Match.AddEntity(projectile);
            }
            base.Kill(gameTime);
        }
    }

    internal class MorningstarSubProjectile : Projectile
    {
        public MorningstarSubProjectile(LuolaGame game, Vector2 position, Vector2 direction, Entity owner, GameTime time)
            : base(game, position, direction, owner, time)
        {
            Damage = 2;
            DestructionSize = 4;
            Speed = 150f;
            Weight = 10;
            Texture = game.Content.Load<Texture2D>("weapons/pellet_projectile.png");
            GraceTime = 0f;
        }
    }
}