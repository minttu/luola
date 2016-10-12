using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace luola.weapons
{
    public class BombWeapon : Weapon
    {
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
        public BombProjectile(Game game, Vector2 position, Vector2 direction, Entity owner) : base(game, position, direction, owner)
        {
            Damage = 10;
            DestructionSize = 32;
            Speed = 0;
            Texture = game.Content.Load<Texture2D>("weapons/bomb_projectile.png");
            Weight = 10;
        }
    }
}