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

namespace Luola.Entities
{
    public abstract class Entity
    {
        public Entity(LuolaGame game)
        {
            Position = Vector2.Zero;
            PreviousPosition = Vector2.Zero;
            IsAlive = true;
            CollideOutside = false;
            Game = game;
        }

        public LuolaGame Game { get; private set; }
        public Match Match { get; set; }
        public Vector2 Position { get; protected set; }
        public Vector2 PreviousPosition { get; protected set; }
        public bool IsAlive { get; protected set; }
        public bool CollideOutside { get; protected set; }

        public virtual void Kill(GameTime gameTime)
        {
            IsAlive = false;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Collided(GameTime gameTime, float x, float y);
    }
}