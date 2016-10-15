using Luola.Entities;
using Microsoft.Xna.Framework;

namespace Luola.Weapons
{
    public abstract class Weapon
    {
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
        public static string Name = "n/a";
        public static float ChanceToAppear = 0;

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