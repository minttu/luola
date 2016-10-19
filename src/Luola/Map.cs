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
using Luola.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public class Map
    {
        private readonly List<Destruction> _destructions;
        private readonly List<MapLayerData> _layers;

        public Map(List<MapLayerData> layers)
        {
            _layers = layers;
            SolidLayer = _layers.Find(layer => layer.Type == "solid");
            DynamicLayer = _layers.Find(layer => layer.Type == "dynamic");
            Width = layers[0].Width;
            Height = layers[0].Height;
            foreach (var layer in layers)
                layer.CalculateCollisions();
            _destructions = new List<Destruction>();
        }

        public int Height { get; set; }
        public int Width { get; set; }
        public List<List<int>> SpawnPoints { get; set; }
        public List<List<int>> PickupPoints { get; set; }
        public Match Match { get; set; }

        private MapLayerData DynamicLayer { get; }
        private MapLayerData SolidLayer { get; }

        public void AddDestruction(Destruction destruction)
        {
            _destructions.Add(destruction);
        }

        public void Update(GameTime gameTime, List<Ship> ships)
        {
            foreach (var destruction in _destructions)
            {
                var particle = destruction.CreateParticle(gameTime);
                if (particle != null)
                    Match.AddParticle(particle);

                for (var y = 0; y < destruction.DestructionType.Size; y++)
                    for (var x = 0; x < destruction.DestructionType.Size; x++)
                    {
                        if (!destruction.DestructionType.EraseData[y, x])
                            continue;

                        var rx = (int) Math.Floor(destruction.Position.X + x);
                        var ry = (int) Math.Floor(destruction.Position.Y + y);
                        if ((rx < 0) || (ry < 0) || (rx >= Width) || (ry >= Height))
                            continue;

                        foreach (var ship in ships)
                        {
                            var point = ship.Position.ToPoint();
                            if ((point.X == rx) && (point.Y == ry) &&
                                ((destruction.Owner != ship) || destruction.FriendlyFire))
                                ship.TakeDamage(destruction.Damage);
                        }

                        DynamicLayer.Colors[rx + ry*Width] = Color.Transparent;
                        DynamicLayer.Collisions[rx, ry] = false;
                    }
            }

            if (_destructions.Count > 0)
                DynamicLayer.Texture.SetData(DynamicLayer.Colors);

            _destructions.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            foreach (var layer in _layers)
                spriteBatch.Draw(layer.Texture, cameraPos*-layer.Parallax, null, Color.White, 0f, Vector2.Zero, 1f,
                    SpriteEffects.None, 0.01f);
        }

        public void CheckCollisions(Entity entity)
        {
            var x = entity.PreviousPosition.X;
            var y = entity.PreviousPosition.Y;
            var delta = entity.Position - entity.PreviousPosition;
            var steps = delta.Length();
            var perStep = delta/steps;

            for (var i = 0; i < steps; i++)
            {
                x += perStep.X;
                y += perStep.Y;

                var overOfMap = (x < 0) || (y < 0) || (x >= Width) || (y >= Height);
                var hit = overOfMap || DynamicLayer.Collisions[(int) x, (int) y] ||
                          (true == SolidLayer?.Collisions[(int) x, (int) y]);

                if (!hit) continue;

                if (entity.CollideOutside)
                {
                    x -= perStep.X;
                    y -= perStep.Y;
                }

                entity.Collided(x, y);
                return;
            }
        }

        public void Dispose()
        {
            foreach (var layer in _layers)
                layer.Dispose();
        }
    }
}