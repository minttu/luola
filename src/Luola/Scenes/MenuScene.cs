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
using System.Linq;
using Luola;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace luola.Scenes
{
    public class MenuScene : Scene
    {
        private readonly List<MenuItem> _items;
        private readonly List<string> _maps;
        private int _itemSelection;
        private int _mapSelection;
        private int _players;

        public MenuScene(LuolaGame game) : base(game)
        {
            _itemSelection = 0;
            _items = new List<MenuItem>();

            var startGame = new MenuItem("start");
            startGame.OnSelected += () =>
            {
                ChangeScene(new MatchScene(game, _players, _maps[_mapSelection]));
                return null;
            };
            _items.Add(startGame);

            _players = 4;
            var selectPlayers = new MenuItem("players: " + _players);
            selectPlayers.OnSelected += () =>
            {
                _players = _players == 1 ? 4 : Math.Max(1, _players - 1);
                return "players: " + _players;
            };
            _items.Add(selectPlayers);

            var dataPath = Path.Combine(Game.Content.RootDirectory, "maps");
            _maps = Directory.EnumerateDirectories(dataPath).Select(s => Path.GetFileName(s)).ToList();
            _mapSelection = 0;
            var selectMap = new MenuItem("map: " + _maps[_mapSelection]);
            selectMap.OnSelected += () =>
            {
                _mapSelection = _mapSelection == _maps.Count - 1 ? 0 : Math.Min(_maps.Count - 1, _mapSelection + 1);
                return "map: " + _maps[_mapSelection];
            };
            _items.Add(selectMap);

            var quit = new MenuItem("quit");
            quit.OnSelected += () =>
            {
                Game.Exit();
                return null;
            };
            _items.Add(quit);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            var width = spriteBatch.GraphicsDevice.Viewport.Width;
            Game.FontManager.DrawText(spriteBatch, "luola", new Vector2(width/2, 100), Color.White,
                4f + (float) Math.Sin(gameTime.TotalGameTime.TotalSeconds)/6, floaty: true,
                gameTime: (float) gameTime.TotalGameTime.TotalSeconds);

            for (var i = 0; i < _items.Count; i++)
            {
                if (i == _itemSelection)
                    spriteBatch.Draw(Game.BaseTexture, new Rectangle(0, 188 + i*30, width, 24), Color.White);
                Game.FontManager.DrawText(spriteBatch, _items[i].Text, new Vector2(width/2, 200 + i*30),
                    _itemSelection == i ? Color.Black : Color.White, 2f);
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.InputManager.IsKeyNewlyDown(Keys.Down))
                _itemSelection = Math.Min(_itemSelection + 1, _items.Count - 1);
            if (Game.InputManager.IsKeyNewlyDown(Keys.Up))
                _itemSelection = Math.Max(_itemSelection - 1, 0);
            if (Game.InputManager.IsKeyNewlyDown(Keys.Enter))
                _items[_itemSelection].Select();
            if (Game.InputManager.IsKeyNewlyDown(Keys.Escape))
                if (_itemSelection != _items.Count - 1)
                    _itemSelection = _items.Count - 1;
                else
                    _items[_itemSelection].Select();
        }

        protected override void Dispose()
        {
        }
    }
}