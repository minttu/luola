using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace luola
{
    public class Destruction
    {
        public Destruction(DestructionType destructionType, Vector2 position, int damage, Entity owner)
        {
            DestructionType = destructionType;
            Position = position - Vector2.One * DestructionType.Size / 2;
            Damage = damage;
            Owner = owner;
        }

        public Entity Owner { get; private set; }
        public int Damage { get; private set; }
        public Vector2 Position { get; private set; }
        public DestructionType DestructionType { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(DestructionType.Texture, Position, Color.Magenta);
        }
    }
}