using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace luola.weapons
{
    public class PelletWeapon : Weapon
    {
        public PelletWeapon(Ship owner) : base(owner)
        {
            TimeBetweenActivations = 0;
        }

        public override void Activate(GameTime gameTime)
        {
            base.Activate(gameTime);

            var projectile = new PelletProjectile(Owner.Game, Owner.Position, Owner.Direction, Owner);
            Owner.Match.AddEntity(projectile);
        }
    }

    internal class PelletProjectile : Projectile
    {
        public PelletProjectile(Game game, Vector2 position, Vector2 direction, Entity owner) : base(game, position, direction, owner)
        {
            Damage = 3;
            DestructionSize = 7;
            Speed = 400f;
            Texture = game.Content.Load<Texture2D>("weapons/pellet_projectile.png");
        }
    }
}