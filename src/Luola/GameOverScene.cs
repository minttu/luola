#region Copyright & License Information
// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Luola
{
    public class GameOverScene : Scene
    {
        private readonly string _winner;

        public GameOverScene(Game game, string winner) : base(game)
        {
            _winner = winner;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            var width = spriteBatch.GraphicsDevice.Viewport.Width;
            var height = spriteBatch.GraphicsDevice.Viewport.Height;
            var str = _winner.Length > 0 ? "Winner is the " + _winner + " player" : "Draw!";
            LuolaGame.FontManager.DrawText(spriteBatch, str, new Vector2(width / 2, height / 2), Color.White,
                4f + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) / 6, floaty: true,
                gameTime: (float)gameTime.TotalGameTime.TotalSeconds);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (LuolaGame.InputManager.IsKeyNewlyDown(Keys.RightShift))
            {
                ChangeScene(new MenuScene(Game));
            }
        }

        public override void Dispose()
        {
        }
    }
}