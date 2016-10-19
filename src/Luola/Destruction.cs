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

namespace Luola
{
    public class Destruction
    {
        public Destruction(DestructionType destructionType, Vector2 position, int damage, Entity owner,
            bool friendlyFire)
        {
            DestructionType = destructionType;
            Position = position - Vector2.One*DestructionType.Size/2;
            Damage = damage;
            Owner = owner;
            FriendlyFire = friendlyFire;
        }

        public Entity Owner { get; }
        public int Damage { get; }
        public Vector2 Position { get; }
        public DestructionType DestructionType { get; }
        public bool FriendlyFire { get; }

        public Particle CreateParticle(GameTime gameTime)
        {
            if (DestructionType.ExplosionTexture == null)
                return null;

            return new Particle(gameTime, Position, DestructionType.ExplosionTexture);
        }
    }
}