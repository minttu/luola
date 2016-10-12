using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace luola
{
    public abstract class Entity
    {
        public Entity(Game game)
        {
            Position = Vector2.Zero;
            PreviousPosition = Vector2.Zero;
            IsAlive = true;
            CollideOutside = false;
            Game = game;
        }

        public Game Game { get; private set; }
        public Match Match { get; set; }
        public Vector2 Position { get; protected set; }
        public Vector2 PreviousPosition { get; protected set; }
        public bool IsAlive { get; protected set; }
        public bool CollideOutside { get; protected set; }

        public void Kill()
        {
            IsAlive = false;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Collided(float x, float y);
    }
}