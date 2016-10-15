using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public abstract class Scene
    {
        public Scene(Game game)
        {
            Game = game;
        }

        public Game Game { get; set; }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
    }
}