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
    public class PelletWeapon : Weapon
    {
        public new static string Name = "pellet";
        public new static float ChanceToAppear = 0f;

        public PelletWeapon(Ship owner) : base(owner)
        {
            TimeBetweenActivations = 0;
            Primary = true;
        }

        public override void Activate(GameTime gameTime)
        {
            base.Activate(gameTime);

            var projectile = new PelletProjectile(Owner.Game, Owner.Position, Owner.Direction, Owner, gameTime);
            Owner.Match.AddEntity(projectile);
        }
    }

    internal class PelletProjectile : Projectile
    {
        public PelletProjectile(Game game, Vector2 position, Vector2 direction, Entity owner, GameTime gameTime)
            : base(game, position, direction, owner, gameTime)
        {
            Damage = 3;
            DestructionSize = 7;
            Speed = 400f;
            Texture = game.Content.Load<Texture2D>("weapons/pellet_projectile.png");
            FriendlyFire = false;
        }
    }
}