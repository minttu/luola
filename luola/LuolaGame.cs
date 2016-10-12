using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace luola
{
    public class LuolaGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;
        private InputManager _inputManager;
        private SpriteBatch _spriteBatch;
        private Match _match;

        public static Texture2D Explosion32;
        public static Texture2D BaseTexture;
        public static DestructionTypeManager DestructionTypeManager;

        public LuolaGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _graphicsDeviceManager.PreferredBackBufferWidth = 1000;
            _graphicsDeviceManager.PreferredBackBufferHeight = 500;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _inputManager = new InputManager();
        }

        private Map LoadMap(string name)
        {
            var dataPath = Path.Combine(Content.RootDirectory, "maps", name, "data.json");
            var stream = TitleContainer.OpenStream(dataPath);
            var streamReader = new StreamReader(stream);
            var data = streamReader.ReadToEnd();
            var mapInfo = JsonConvert.DeserializeObject<MapData>(data);

            foreach (var layer in mapInfo.Layers)
            {
                layer.Texture = Content.Load<Texture2D>(Path.Combine("maps", name, layer.Image));
            }

            return new Map(mapInfo.Layers)
            {
                SpawnPoints = mapInfo.SpawnPoints
            };
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            DestructionTypeManager = new DestructionTypeManager(GraphicsDevice);

            var map = LoadMap("a1");
            _match = new Match(this, map);
            _match.CreateShip(Color.Blue);
            _match.CreateShip(Color.Red);

            BaseTexture = new Texture2D(GraphicsDevice, 1, 1);
            BaseTexture.SetData(new Color[] {Color.White});

            Explosion32 = Content.Load<Texture2D>("explosions/32");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _inputManager.Update(gameTime);

            _inputManager.InputForShip(_match.Ships[0], 0);
            _inputManager.InputForShip(_match.Ships[1], 1);

            _match.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            var mapHeight = _match.MapSize.X;
            var mapWidth = _match.MapSize.Y;
            var each = _graphicsDeviceManager.PreferredBackBufferWidth/_match.Ships.Count;
            each = Math.Min(each, mapWidth);


            for (var i = 0; i < _match.Ships.Count; i++)
            {
                var viewportRect = new Rectangle(i*each, 0, each,
                    Math.Min(mapHeight, _graphicsDeviceManager.PreferredBackBufferHeight));
                viewportRect.Inflate(-1, -1);
                GraphicsDevice.Viewport = new Viewport(viewportRect);
                var ship = _match.Ships[i];
                var shipPos = ship.Position.ToPoint().ToVector2();
                var cameraPos = -(shipPos - GraphicsDevice.Viewport.Bounds.Center.ToVector2()) - Vector2.UnitX*(i*each);

                cameraPos = new Vector2(MathHelper.Clamp(cameraPos.X, -mapWidth + GraphicsDevice.Viewport.Width, 0f),
                    MathHelper.Clamp(cameraPos.Y, -mapHeight + GraphicsDevice.Viewport.Height, 0f));
                var transformMatrix = Matrix.CreateTranslation(new Vector3(cameraPos, 0f));
                _spriteBatch.Begin(transformMatrix: transformMatrix, blendState: BlendState.NonPremultiplied,
                    sortMode: SpriteSortMode.Deferred);

                _match.Draw(gameTime, _spriteBatch, cameraPos);

                _spriteBatch.End();

                _spriteBatch.Begin();
                _spriteBatch.Draw(BaseTexture,
                    new Rectangle(0, 0, (int) (((float) ship.Health/(float) ship.MaxHealth)*each), 20), ship.Color);
                _spriteBatch.End();
            }
        }
    }
}