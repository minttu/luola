using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace luola.weapons
{
    public class MorningstarWeapon : Weapon
    {
        public MorningstarWeapon(Ship owner) : base(owner)
        {
            TimeBetweenActivations = 1f;
        }

        public override void Activate(GameTime gameTime)
        {
            base.Activate(gameTime);
            int projectiles = 32;
            for (int i = 0; i < projectiles; i++)
            {
                var dir = Vector2.Transform(-Vector2.UnitY, Matrix.CreateRotationZ((float) (Math.PI*2/projectiles*i)));
                var projectile = new MorningstarProjectile(Owner.Game, Owner.Position, dir, Owner);
                Owner.Match.AddEntity(projectile);
            }
        }
    }

    internal class MorningstarProjectile : Projectile
    {
        public MorningstarProjectile(Game game, Vector2 position, Vector2 direction, Entity owner)
            : base(game, position, direction, owner)
        {
            Damage = 2;
            DestructionSize = 6;
            Speed = 150f;
            Texture = game.Content.Load<Texture2D>("weapons/pellet_projectile.png");
        }
    }
}