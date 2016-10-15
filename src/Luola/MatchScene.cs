using System;
using System.Collections.Generic;
using System.IO;
using luola.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Luola
{
    public class MatchScene : Scene
    {
        private readonly InputManager _inputManager;

        private Match _match;
        private readonly List<ShipView> _shipViews;

        public MatchScene(Game game) : base(game)
        {
            _inputManager = new InputManager();
            _shipViews = new List<ShipView>();

            var map = LoadMap("a1");
            _match = new Match(Game, map);
            _match.CreateShip(Color.Blue);
            _match.CreateShip(Color.Red);
            _match.CreateShip(Color.Green);
            _match.CreateShip(Color.Yellow);

            foreach (var ship in _match.Ships)
                _shipViews.Add(new ShipView(Game, ship));
        }

        private Map LoadMap(string name)
        {
            var dataPath = Path.Combine(Game.Content.RootDirectory, "maps", name, "data.json");
            var stream = TitleContainer.OpenStream(dataPath);
            var streamReader = new StreamReader(stream);
            var data = streamReader.ReadToEnd();
            var mapInfo = JsonConvert.DeserializeObject<MapData>(data);

            foreach (var layer in mapInfo.Layers)
            {
                layer.Texture = Game.Content.Load<Texture2D>(Path.Combine("maps", name, layer.Image));
            }

            return new Map(mapInfo.Layers)
            {
                SpawnPoints = mapInfo.SpawnPoints,
                PickupPoints = mapInfo.PickupPoints
            };
        }

        public override void Update(GameTime gameTime)
        {
            _inputManager.Update(gameTime);

            _inputManager.InputForShip(_match.Ships[0], 0);
            _inputManager.InputForShip(_match.Ships[1], 1);

            _match.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var mapHeight = _match.MapSize.X;
            var mapWidth = _match.MapSize.Y;
            var eachWidth = ((LuolaGame) Game).GraphicsDeviceManager.PreferredBackBufferWidth/2;
            eachWidth = Math.Min(eachWidth, mapWidth);
            var eachHeight = ((LuolaGame) Game).GraphicsDeviceManager.PreferredBackBufferHeight/2;
            eachHeight = Math.Min(eachHeight, mapHeight);

            for (var i = 0; i < _match.Ships.Count; i++)
            {
                var x = i%2;
                int y = i/2;
                var viewportRect = new Rectangle(x*eachWidth,
                    y*eachHeight,
                    eachWidth,
                    eachHeight);
                viewportRect.Inflate(-1, -1);
                Game.GraphicsDevice.Viewport = new Viewport(viewportRect);

                _shipViews[i].Draw(gameTime, spriteBatch, x, y);
            }
        }
    }
}