#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Luola
{
    public class DestructionTypeManager
    {
        private readonly Dictionary<int, DestructionType> _destructionTypes;
        private readonly Game _game;

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