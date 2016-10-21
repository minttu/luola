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

namespace Luola
{
    public class Particle
    {
        private readonly float _createdAt;
        private readonly float _duration;
        private readonly bool _isAnimated;
        private readonly Texture2D _texture;
        private Vector2 _position;
        public bool IsAlive;

        public Particle(GameTime gameTime, Vector2 position, Texture2D texture)
        {
            _createdAt = (float) gameTime.TotalGameTime.TotalSeconds;
            IsAlive = true;
            _duration = 0.3f;
            _isAnimated = texture.Width != texture.Height;
            _position = position;
            _texture = texture;
        }

        private float ExpiresAt => _createdAt + _duration;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (ExpiresAt < (float) gameTime.TotalGameTime.TotalSeconds)
            {
                IsAlive = false;
                return;
            }

            if (!_isAnimated)
            {
                spriteBatch.Draw(_texture, _position, Color.White);
                return;
            }

            var height = _texture.Height;
            var frames = _texture.Width/height;
            var width = height;
            var timePerFrame = _duration/frames;
            var iter = (int) Math.Floor((gameTime.TotalGameTime.TotalSeconds - _createdAt)/timePerFrame);
            if (iter >= frames)
            {
                IsAlive = false;
                return;
            }
            spriteBatch.Draw(_texture, new Rectangle(_position.ToPoint(), new Point(width, height)),
                new Rectangle(width*iter, 0, width, height), Color.White);
        }
    }
}