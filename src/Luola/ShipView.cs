﻿using Luola.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public class ShipView
    {
        private readonly Ship _ship;
        private readonly Game _game;

        public ShipView(Game game, Ship ship)
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
            spriteBatch.Begin(transformMatrix: transformMatrix, blendState: BlendState.NonPremultiplied,
                sortMode: SpriteSortMode.Deferred);

            _ship.Match.Draw(gameTime, spriteBatch, cameraPos);

            spriteBatch.End();
        }

        private void DrawHud(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            var rectangle = new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width, 24);
            spriteBatch.Draw(LuolaGame.BaseTexture, rectangle, Color.Black);
            rectangle = new Rectangle(0, 2,
                (int) (((float) _ship.Health/(float) _ship.MaxHealth)*spriteBatch.GraphicsDevice.Viewport.Width), 20);
            spriteBatch.Draw(LuolaGame.BaseTexture, rectangle, Color.White);

            spriteBatch.End();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, int x, int y)
        {
            DrawMatch(gameTime, spriteBatch, x, y);
            DrawHud(gameTime, spriteBatch);
        }
    }
}