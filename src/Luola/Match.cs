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
using luola;
using Luola.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public class Match
    {
        private readonly List<MapArea> _areas;
        private readonly List<BaseArea> _bases;
        private readonly List<Destruction> _destructions;
        private readonly LuolaGame _game;
        private readonly List<MapLayer> _layers;
        private readonly List<MapPoint> _points;
        private readonly Random _random;
        public readonly List<Ship> Ships;
        private List<Entity> _entities;
        private List<Particle> _particles;

        public Match(LuolaGame game, MapData mapData)
        {
            _game = game;
            _areas = mapData.Areas;
            _points = mapData.Points;

            _entities = new List<Entity>();
            _particles = new List<Particle>();
            _bases = new List<BaseArea>();
            _destructions = new List<Destruction>();
            _random = new Random();
            Ships = new List<Ship>();

            _layers = mapData.Layers;
            SolidLayer = _layers.Find(layer => layer.Type == "solid");
            DynamicLayer = _layers.Find(layer => layer.Type == "dynamic");
            Width = _layers[0].Width;
            Height = _layers[0].Height;
            foreach (var layer in _layers)
                layer.CalculateCollisions();

            CreatePickups();
            CreateBases();
        }

        private int Height { get; }
        private int Width { get; }
        private MapLayer DynamicLayer { get; }
        private MapLayer SolidLayer { get; }

        public Point MapSize => new Point(Width, Height);
        public List<Ship> AliveShips => Ships.FindAll(s => s.IsAlive);

        private void AddParticle(Particle particle)
        {
            _particles.Add(particle);
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
            entity.Match = this;
        }

        private List<MapPoint> PointsOfType(string type)
        {
            return _points.FindAll(t => t.Type == type);
        }

        private MapPoint RandomPointOfType(Random random, string type)
        {
            var points = PointsOfType(type);
            return points[random.Next(0, points.Count)];
        }

        public void CreateShip(string name, Color color)
        {
            var spawnPoint = RandomPointOfType(_random, "spawn");
            var ship = new Ship(_game, name, color, spawnPoint.AsVector);
            AddEntity(ship);
            Ships.Add(ship);
        }

        private void CreatePickups()
        {
            foreach (var pos in PointsOfType("pickup"))
            {
                var pickup = new Pickup(_game, pos.AsVector);
                AddEntity(pickup);
            }
        }

        private void CreateBases()
        {
            foreach (var area in _areas.FindAll(t => t.Type == "base"))
                _bases.Add(new BaseArea(area.AsRectangle));
        }

        public void AddDestruction(Destruction destruction)
        {
            _destructions.Add(destruction);
        }

        private void CalculateDestructions(GameTime gameTime)
        {
            if (_destructions.Count == 0)
                return;

            foreach (var destruction in _destructions)
            {
                var particle = destruction.CreateParticle(gameTime);
                if (particle != null)
                    AddParticle(particle);

                for (var y = 0; y < destruction.DestructionType.Size; y++)
                    for (var x = 0; x < destruction.DestructionType.Size; x++)
                    {
                        if (!destruction.DestructionType.EraseData[y, x])
                            continue;

                        var rx = (int) Math.Floor(destruction.Position.X + x);
                        var ry = (int) Math.Floor(destruction.Position.Y + y);
                        if ((rx < 0) || (ry < 0) || (rx >= Width) || (ry >= Height))
                            continue;

                        foreach (var ship in AliveShips)
                        {
                            var point = ship.Position.ToPoint();
                            if ((point.X == rx) && (point.Y == ry) &&
                                ((destruction.Owner != ship) || destruction.FriendlyFire))
                                ship.TakeDamage(gameTime, destruction.Damage);
                        }

                        DynamicLayer.Colors[rx + ry*Width] = Color.Transparent;
                        DynamicLayer.Collisions[rx, ry] = false;
                    }
            }

            DynamicLayer.UpdateTextureFromColors();
            _destructions.Clear();
        }

        private void CheckForCollisionWithLayers(GameTime gameTime, Entity entity)
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

                entity.Collided(gameTime, x, y);
                return;
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in _entities.ToArray())
            {
                entity.Update(gameTime);
                CheckForCollisionWithLayers(gameTime, entity);
                if (entity is Ship)
                    continue;

                foreach (var ship in AliveShips)
                    ship.CheckCollisionWithEntity(gameTime, entity);
            }
            _entities = _entities.FindAll(entity => entity.IsAlive);

            CalculateDestructions(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            foreach (var layer in _layers)
                layer.Draw(gameTime, spriteBatch, cameraPos);

            foreach (var entity in _entities)
                entity.Draw(gameTime, spriteBatch);

            foreach (var particle in _particles)
                particle.Draw(gameTime, spriteBatch);

            _particles = _particles.FindAll(particle => particle.IsAlive);
        }

        public void Dispose()
        {
            foreach (var layer in _layers)
                layer.Dispose();
        }
    }
}