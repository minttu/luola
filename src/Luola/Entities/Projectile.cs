using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola.Entities
{
    public abstract class Projectile : Entity
    {
        public Projectile(Game game, Vector2 position, Vector2 direction, Entity owner) : base(game)
        {
            Speed = 400f;
            Position = position;
            Direction = direction;
            IsAlive = true;
            Owner = owner;
            Damage = 0;
            DestructionSize = 1;
            Weight = 1;
            CollideOutside = false;
        }

        public int Weight { get; set; }

        protected int DestructionSize { get; set; }
        public int Damage { get; protected set; }

        protected float Speed
        {
            set { Velocity = Direction*value; }
        }

        protected Vector2 Velocity { get; set; }
        public Entity Owner { get; }
        protected Texture2D Texture { get; set; }
        protected Vector2 Direction { get; set; }

        private Rectangle Rectangle
            =>
            new Rectangle((Position - Vector2.One*(Texture.Width/2)).ToPoint(), new Point(Texture.Width, Texture.Height))
            ;

        public override void Update(GameTime gameTime)
        {
            PreviousPosition = Position;
            Velocity += Vector2.UnitY/4*Weight;
            Position += Velocity*(float) gameTime.ElapsedGameTime.TotalSeconds;
            Velocity *= 0.999f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }

        public override void Collided(float x, float y)
        {
            IsAlive = false;
            Match.AddDestruction(new Destruction(LuolaGame.DestructionTypeManager.GetDestructionType(DestructionSize),
                new Vector2(x, y), Damage, Owner));
        }
    }
}