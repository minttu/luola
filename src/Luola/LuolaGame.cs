using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using luola.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Luola
{
    public class LuolaGame : Game
    {
        public readonly GraphicsDeviceManager GraphicsDeviceManager;

        private SpriteBatch _spriteBatch;
        public static Texture2D BaseTexture;
        public static DestructionTypeManager DestructionTypeManager;
        public static WeaponManager WeaponManager;

        public Scene Scene { get; set; }

        public LuolaGame()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.PreferredBackBufferWidth = 1000;
            GraphicsDeviceManager.PreferredBackBufferHeight = 1000;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            DestructionTypeManager = new DestructionTypeManager(this);

            BaseTexture = new Texture2D(GraphicsDevice, 1, 1);
            BaseTexture.SetData(new[] {Color.White});

            WeaponManager = new WeaponManager();

            Scene = new MatchScene(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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