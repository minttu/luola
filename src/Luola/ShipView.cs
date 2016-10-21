#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using Luola.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public class ShipView
    {
        private readonly LuolaGame _game;
        private readonly Ship _ship;

        public ShipView(LuolaGame game, Ship ship)
        {
            _game = game;
            _ship = ship;
        }

        private void DrawMatch(GameTime gameTime, SpriteBatch spriteBatch, int x, int y)
        {
            var mapWidth = _ship.Match.MapSize.X;
            var mapHeight = _ship.Match.MapSize.Y;

            var shipPos = _ship.Position.ToPoint().ToVector2();
            var cameraPos = -(shipPos - _game.GraphicsDevice.Viewport.Bounds.Center.ToVector2()) -
                            Vector2.UnitX*(x*spriteBatch.GraphicsDevice.Viewport.Width) -
                            Vector2.UnitY*(y*spriteBatch.GraphicsDevice.Viewport.Height);

            cameraPos = new Vector2(MathHelper.Clamp(cameraPos.X, -mapWidth + _game.GraphicsDevice.Viewport.Width, 0f),
                MathHelper.Clamp(cameraPos.Y, -mapHeight + _game.GraphicsDevice.Viewport.Height, 0f));
            var transformMatrix = Matrix.CreateTranslation(new Vector3(cameraPos, 0f));
            spriteBatch.Begin(transformMatrix: transformMatrix, blendState: BlendState.NonPremultiplied);

            _ship.Match.Draw(gameTime, spriteBatch, cameraPos);

            spriteBatch.End();
        }

        private void DrawHud(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            var rectangle = new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width, 24);
            spriteBatch.Draw(_game.BaseTexture, rectangle, Color.Black);
            rectangle = new Rectangle(0, 2,
                (int) (_ship.Health/(float) _ship.MaxHealth*spriteBatch.GraphicsDevice.Viewport.Width), 20);
            spriteBatch.Draw(_game.BaseTexture, rectangle, Color.White);

            _game.FontManager.DrawText(spriteBatch, _ship.Health + "/" + _ship.MaxHealth,
                new Vector2(spriteBatch.GraphicsDevice.Viewport.Width/2, 11), Color.Black, 2f, 0);

            spriteBatch.End();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, int x, int y)
        {
            DrawMatch(gameTime, spriteBatch, x, y);
            DrawHud(gameTime, spriteBatch);
        }
    }
}