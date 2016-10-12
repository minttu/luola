using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace luola
{
    public class DestructionTypeManager
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Dictionary<int, DestructionType> _destructionTypes;

        public DestructionTypeManager(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _destructionTypes = new Dictionary<int, DestructionType>();
        }

        public DestructionType GetDestructionType(int size)
        {
            if (!_destructionTypes.ContainsKey(size))
                _destructionTypes[size] = new DestructionType(_graphicsDevice, size);

            return _destructionTypes[size];
        }
    }
}