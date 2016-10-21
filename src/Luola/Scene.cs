#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public abstract class Scene
    {
        protected Scene(LuolaGame game)
        {
            Game = game;
        }

        protected LuolaGame Game { get; }

        protected void ChangeScene(Scene scene)
        {
            Dispose();
            Game.Scene = scene;
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        protected abstract void Dispose();
    }
}