using Microsoft.Xna.Framework;

namespace luola
{
    public abstract class Weapon
    {
        private float _lastActivated;

        public Weapon(Ship owner)
        {
            _lastActivated = 0;
            Owner = owner;
        }

        protected float TimeBetweenActivations { get; set; }

        protected Ship Owner { get; private set; }

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