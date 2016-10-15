using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace luola
{
    public class DestructionTypeManager
    {
        private readonly Dictionary<int, DestructionType> _destructionTypes;
        private Game _game;

        public DestructionTypeManager(Game game)
        {
            _game = game;
            _destructionTypes = new Dictionary<int, DestructionType>();
        }

        public DestructionType GetDestructionType(int size)
        {
            if (!_destructionTypes.ContainsKey(size))
                _destructionTypes[size] = new DestructionType(_game, size);

            return _destructionTypes[size];
        }
    }
}