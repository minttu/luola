﻿#region Copyright & License Information

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
            TimeBetweenActivations = 1f;
        }

        public override void Activate(GameTime gameTime)
        {
            base.Activate(gameTime);

            var projectile = new MorningstarProjectile(Owner.Game, Owner.FrontPosition, Owner.Direction, Owner);
            Owner.Match.AddEntity(projectile);
        }
    }

    internal class MorningstarProjectile : Projectile
    {
        public MorningstarProjectile(Game game, Vector2 position, Vector2 direction, Entity owner)
            : base(game, position, direction, owner)
        {
            Damage = 20;
            DestructionSize = 32;
            Speed = 250f;
            Texture = game.Content.Load<Texture2D>("weapons/morningstar_projectile.png");
        }

        public override void Kill()
        {
            const int projectiles = 32;
            for (var i = 0; i < projectiles; i++)
            {
                var dir = Vector2.Transform(-Vector2.UnitY, Matrix.CreateRotationZ((float)(Math.PI * 2 / projectiles * i)));
                var projectile = new MorningstarSubProjectile(Owner.Game, Position, dir, Owner);
                Owner.Match.AddEntity(projectile);
            }
            base.Kill();
        }
    }

    internal class MorningstarSubProjectile : Projectile
    {
        public MorningstarSubProjectile(Game game, Vector2 position, Vector2 direction, Entity owner)
            : base(game, position, direction, owner)
        {
            Damage = 2;
            DestructionSize = 4;
            Speed = 150f;
            Weight = 10;
            Texture = game.Content.Load<Texture2D>("weapons/pellet_projectile.png");
        }
    }
}