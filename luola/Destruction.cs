using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace luola
{
    public class Destruction
    {
        public Destruction(DestructionType destructionType, Vector2 position, int damage, Entity owner)
        {
            DestructionType = destructionType;
            Position = position - Vector2.One*DestructionType.Size/2;
            Damage = damage;
            Owner = owner;
        }

        public Entity Owner { get; }
        public int Damage { get; }
        public Vector2 Position { get; }
        public DestructionType DestructionType { get; }

        public Particle CreateParticle(GameTime gameTime)
        {
            if (DestructionType.ExplosionTexture == null)
                return null;

            return new Particle(gameTime, Position, DestructionType.ExplosionTexture);
        }
    }
}