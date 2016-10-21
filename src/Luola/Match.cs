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
    public class Match
    {
        private readonly Game _game;
        private readonly Map _map;
        private readonly Random _random;
        private List<Entity> _entities;
        private List<Particle> _particles;
        public List<Ship> Ships;

        public Match(Game game, Map map)
        {
            _game = game;
            _map = map;
            _map.Match = this;
            _entities = new List<Entity>();
            _particles = new List<Particle>();
            _random = new Random();
            Ships = new List<Ship>();

            CreatePickups();
        }

        public Point MapSize => new Point(_map.Width, _map.Height);
        public List<Ship> AliveShips => Ships.FindAll(s => s.IsAlive);

        public void AddParticle(Particle particle)
        {
            _particles.Add(particle);
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
            entity.Match = this;
        }

        public void CreateShip(string name, Color color)
        {
            var spawnPoint = _map.RandomPointOfType(_random, "spawn");
            var ship = new Ship(_game, name, color, spawnPoint.AsVector);
            AddEntity(ship);
            Ships.Add(ship);
        }

        private void CreatePickups()
        {
            foreach (var pos in _map.PointsOfType("pickup"))
            {
                var pickup = new Pickup(_game, pos.AsVector);
                AddEntity(pickup);
            }
        }

        public void AddDestruction(Destruction destruction)
        {
            _map.AddDestruction(destruction);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in _entities.ToArray())
            {
                entity.Update(gameTime);
                _map.CheckCollisions(gameTime, entity);
                if (entity is Ship)
                    continue;

                foreach (var ship in AliveShips)
                    ship.CheckCollisionWithEntity(gameTime, entity);
            }
            _entities = _entities.FindAll(entity => entity.IsAlive);

            _map.Update(gameTime, AliveShips);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            _map.Draw(gameTime, spriteBatch, cameraPos);
            foreach (var entity in _entities)
                entity.Draw(gameTime, spriteBatch);

            foreach (var particle in _particles)
                particle.Draw(gameTime, spriteBatch);
            _particles = _particles.FindAll(particle => particle.IsAlive);
        }

        public void Dispose()
        {
            _map.Dispose();
        }
    }
}