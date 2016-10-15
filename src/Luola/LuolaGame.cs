using Luola.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public class LuolaGame : Game
    {
        public static Texture2D BaseTexture;
        public static DestructionTypeManager DestructionTypeManager;
        public static WeaponManager WeaponManager;
        public static FontManager FontManager;
        public static InputManager InputManager;
        public readonly GraphicsDeviceManager GraphicsDeviceManager;

        private SpriteBatch _spriteBatch;

        public LuolaGame()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.PreferredBackBufferWidth = 1000;
            GraphicsDeviceManager.PreferredBackBufferHeight = 1000;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public Scene Scene { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            BaseTexture = new Texture2D(GraphicsDevice, 1, 1);
            BaseTexture.SetData(new[] {Color.White});

            DestructionTypeManager = new DestructionTypeManager(this);
            WeaponManager = new WeaponManager();
            FontManager = new FontManager(this);
            InputManager = new InputManager();
            Scene = new MenuScene(this);
            //Scene = new MatchScene(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            InputManager.Update(gameTime);
            Scene.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
            Scene.Draw(gameTime, _spriteBatch);
        }
    }
}