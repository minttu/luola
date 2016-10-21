#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using Luola;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace luola.Scenes
{
    public class MatchScene : Scene
    {
        private readonly Match _match;
        private readonly List<ShipView> _shipViews;

        public MatchScene(LuolaGame game, int players, string mapName) : base(game)
        {
            _shipViews = new List<ShipView>();

            var map = LoadMap(mapName);
            _match = new Match(Game, map);
            var playerColors = new[] {Color.Blue, Color.Red, Color.Green, Color.Yellow};
            var playerNames = new[] {"Blue", "Red", "Green", "Yellow"};

            for (var i = 0; i < players; i++)
                _match.CreateShip(playerNames[i], playerColors[i]);

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

            return new Map(mapInfo);
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.InputManager.IsKeyNewlyDown(Keys.Escape))
            {
                ChangeScene(new MenuScene(Game));
                return;
            }

            for (var i = 0; i < _match.Ships.Count; i++)
                Game.InputManager.InputForShip(_match.Ships[i], i);

            _match.Update(gameTime);

            if ((_match.AliveShips.Count == 1) && (_match.Ships.Count > 1))
                ChangeScene(new GameOverScene(Game, _match.AliveShips[0].Name));
            if (_match.AliveShips.Count == 0)
                ChangeScene(new GameOverScene(Game, ""));
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
            var eachWidth = Game.GraphicsDeviceManager.PreferredBackBufferWidth/split.Item1;
            eachWidth = Math.Min(eachWidth, mapWidth);
            var eachHeight = Game.GraphicsDeviceManager.PreferredBackBufferHeight/split.Item2;
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

        protected override void Dispose()
        {
            var width = Game.GraphicsDeviceManager.PreferredBackBufferWidth;
            var height = Game.GraphicsDeviceManager.PreferredBackBufferHeight;
            Game.GraphicsDevice.Viewport = new Viewport(new Rectangle(0, 0, width, height));
            _match.Dispose();
        }
    }
}