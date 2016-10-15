using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Luola.Entities;

namespace Luola
{
    public class Match
    {
        private readonly Game _game;
        private readonly Map _map;
        private List<Entity> _entities;
        private List<Particle> _particles;
        private Random _random;

        public Point MapSize => new Point(_map.Width, _map.Height);
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

        public void AddParticle(Particle particle)
        {
            _particles.Add(particle);
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
            entity.Match = this;
        }

        public void CreateShip(Color color)
        {
            var spawnPoint = _map.SpawnPoints[_random.Next(0, _map.SpawnPoints.Count)];
            var ship = new Ship(_game, color, new Vector2(spawnPoint[0], spawnPoint[1]));
            AddEntity(ship);
            Ships.Add(ship);
        }

        private void CreatePickups()
        {
            foreach (var pos in _map.PickupPoints)
            {
                var pickup = new Pickup(_game, new Vector2(pos[0], pos[1]));
                AddEntity(pickup);
            }
        }

        public void AddDestruction(Destruction destruction)
        {
            _map.AddDestruction(destruction);
        }

        public void Update(GameTime gameTime)
        {
            foreach (Entity entity in _entities)
            {
                entity.Update(gameTime);
                _map.CheckCollisions(entity);
                if (entity is Ship)
                    continue;

                foreach (var ship in Ships)
                {
                    var projectile = entity as Projectile;
                    if (projectile != null)
                    {
                        var bullet = projectile;
                        if (bullet.Owner == ship)
                            continue;

                        ship.CheckCollisionsWithProjectile(projectile);
                    }

                    var pickup = entity as Pickup;
                    if (pickup != null && pickup.Active)
                    {
                        ship.CheckCollisionsWithPickup(pickup);
                    }
                }
            }
            _entities = _entities.FindAll((entity) => entity.IsAlive);

            _map.Update(gameTime, Ships);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            _map.Draw(gameTime, spriteBatch, cameraPos);
            foreach (Entity entity in _entities)
            {
                entity.Draw(gameTime, spriteBatch);
            }

            foreach (var particle in _particles)
            {
                particle.Draw(gameTime, spriteBatch);
            }
            _particles = _particles.FindAll((particle => particle.IsAlive));
        }
    }
}