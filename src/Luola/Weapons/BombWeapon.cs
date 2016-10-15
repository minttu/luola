#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using Luola.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola.Weapons
{
    public class BombWeapon : Weapon
    {
        public new static string Name = "bomb";
        public new static float ChanceToAppear = 1f;

        public BombWeapon(Ship owner) : base(owner)
        {
            TimeBetweenActivations = 1f;
        }

        public override void Activate(GameTime gameTime)
        {
            base.Activate(gameTime);

            var projectile = new BombProjectile(Owner.Game, Owner.Position, Owner.Direction, Owner);
            Owner.Match.AddEntity(projectile);
        }
    }

    internal class BombProjectile : Projectile
    {
        public BombProjectile(Game game, Vector2 position, Vector2 direction, Entity owner)
            : base(game, position, direction, owner)
        {
            Damage = 10;
            DestructionSize = 32;
            Speed = 0;
            Texture = game.Content.Load<Texture2D>("weapons/bomb_projectile.png");
            Weight = 10;
        }
    }
}