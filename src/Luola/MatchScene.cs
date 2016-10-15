using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Luola
{
    public class MatchScene : Scene
    {
        private readonly List<ShipView> _shipViews;
        private readonly Match _match;

        public MatchScene(Game game, int players, string mapName) : base(game)
        {
            _shipViews = new List<ShipView>();

            var map = LoadMap(mapName);
            _match = new Match(Game, map);
            var playerColors = new[] {Color.Blue, Color.Red, Color.Green, Color.Yellow};

            for (var i = 0; i < players; i++)
                _match.CreateShip(playerColors[i]);

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
                var texture = Game.Content.Load<Texture2D>(Path.Combine("maps", name, layer.Image));
                var copy = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
                var textureData = new Color[texture.Width*texture.Height];
                texture.GetData(textureData);
                copy.SetData(textureData);
                layer.Texture = copy;
            }

            return new Map(mapInfo.Layers)
            {
                SpawnPoints = mapInfo.SpawnPoints,
                PickupPoints = mapInfo.PickupPoints
            };
        }

        public override void Update(GameTime gameTime)
        {
            if (LuolaGame.InputManager.IsKeyNewlyDown(Keys.Escape))
            {
                ChangeScene(new MenuScene(Game));
                return;
            }

            for (var i = 0; i < _match.Ships.Count; i++)
                LuolaGame.InputManager.InputForShip(_match.Ships[i], i);

            _match.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var splits = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(2, 1),
                new Tuple<int, int>(3, 1),
                new Tuple<int, int>(2, 2)
            };
            var split = splits[_match.Ships.Count - 1];

            var mapHeight = _match.MapSize.X;
            var mapWidth = _match.MapSize.Y;
            var eachWidth = ((LuolaGame) Game).GraphicsDeviceManager.PreferredBackBufferWidth/split.Item1;
            eachWidth = Math.Min(eachWidth, mapWidth);
            var eachHeight = ((LuolaGame) Game).GraphicsDeviceManager.PreferredBackBufferHeight/split.Item2;
            eachHeight = Math.Min(eachHeight, mapHeight);

            for (var i = 0; i < _match.Ships.Count; i++)
            {
                var x = i%split.Item1;
                var y = split.Item2 == 1 ? 0 : i/split.Item2;
                var viewportRect = new Rectangle(x*eachWidth,
                    y*eachHeight,
                    eachWidth,
                    eachHeight);
                viewportRect.Inflate(-1, -1);
                Game.GraphicsDevice.Viewport = new Viewport(viewportRect);

                _shipViews[i].Draw(gameTime, spriteBatch, x, y);
            }
        }

        public override void Dispose()
        {
            var width = ((LuolaGame) Game).GraphicsDeviceManager.PreferredBackBufferWidth;
            var height = ((LuolaGame) Game).GraphicsDeviceManager.PreferredBackBufferHeight;
            Game.GraphicsDevice.Viewport = new Viewport(new Rectangle(0, 0, width, height));
            _match.Dispose();
        }
    }
}