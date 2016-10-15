using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Luola
{
    public class MenuScene : Scene
    {
        private readonly List<MenuItem> _items;
        private int _itemSelection;
        private readonly List<string> _maps;
        private int _mapSelection;
        private int _players;

        public MenuScene(Game game) : base(game)
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
            LuolaGame.FontManager.DrawText(spriteBatch, "luola", new Vector2(width/2, 100), Color.White,
                4f + (float) Math.Sin(gameTime.TotalGameTime.TotalSeconds)/6, floaty: true,
                gameTime: (float) gameTime.TotalGameTime.TotalSeconds);

            for (var i = 0; i < _items.Count; i++)
            {
                if (i == _itemSelection)
                    spriteBatch.Draw(LuolaGame.BaseTexture, new Rectangle(0, 188 + i*30, width, 24), Color.White);
                LuolaGame.FontManager.DrawText(spriteBatch, _items[i].Text, new Vector2(width/2, 200 + i*30),
                    _itemSelection == i ? Color.Black : Color.White, 2f);
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (LuolaGame.InputManager.IsKeyNewlyDown(Keys.Down))
                _itemSelection = Math.Min(_itemSelection + 1, _items.Count - 1);
            if (LuolaGame.InputManager.IsKeyNewlyDown(Keys.Up))
                _itemSelection = Math.Max(_itemSelection - 1, 0);
            if (LuolaGame.InputManager.IsKeyNewlyDown(Keys.RightShift))
                _items[_itemSelection].Select();
            if (LuolaGame.InputManager.IsKeyNewlyDown(Keys.Escape))
                if (_itemSelection != _items.Count - 1)
                    _itemSelection = _items.Count - 1;
                else
                    _items[_itemSelection].Select();
        }

        public override void Dispose()
        {
        }
    }
}