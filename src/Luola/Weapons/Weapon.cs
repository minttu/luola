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

namespace Luola.Weapons
{
    public abstract class Weapon
    {
        public static string Name = "n/a";
        public static float ChanceToAppear = 0;
        private float _lastActivated;

        public Weapon(Ship owner)
        {
            _lastActivated = 0;
            Owner = owner;
            Primary = false;
        }

        protected float TimeBetweenActivations { get; set; }
        protected Ship Owner { get; private set; }
        public bool Primary { get; protected set; }

        public virtual void Activate(GameTime gameTime)
        {
            _lastActivated = (float) gameTime.TotalGameTime.TotalSeconds;
        }

        public bool CanActivate(GameTime gameTime)
        {
            return _lastActivated + TimeBetweenActivations <= (float) gameTime.TotalGameTime.TotalSeconds;
        }
    }
}