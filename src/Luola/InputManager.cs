#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

using System.Collections.Generic;
using Luola.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Luola
{
    public class InputManager
    {
        private readonly Dictionary<int, Keys[]> _playerKeys;
        private GameTime _gameTime;
        private KeyboardState _keyboardState;
        private KeyboardState _oldKeyboardState;

        public InputManager()
        {
            _oldKeyboardState = Keyboard.GetState();
            _playerKeys = new Dictionary<int, Keys[]>
            {
                [0] = new[] {Keys.Up, Keys.Left, Keys.Right, Keys.RightShift, Keys.Down},
                [1] = new[] {Keys.W, Keys.A, Keys.D, Keys.LeftShift, Keys.S},
                [2] = new[] {Keys.Y, Keys.G, Keys.J, Keys.F, Keys.H},
                [3] = new[] {Keys.L, Keys.OemComma, Keys.OemQuestion, Keys.M, Keys.OemPeriod}
            };
        }

        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            _oldKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key);
        }

        public bool IsKeyNewlyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key) && _oldKeyboardState.IsKeyUp(key);
        }

        public void InputForShip(Ship ship, int index)
        {
            var rotation = 0f;
            var thrust = 0f;

            if (index >= _playerKeys.Count)
                return;

            if (IsKeyDown(_playerKeys[index][0]))
                thrust = 1f;
            if (IsKeyDown(_playerKeys[index][1]))
                rotation = -1f;
            if (IsKeyDown(_playerKeys[index][2]))
                rotation = 1f;
            if (IsKeyNewlyDown(_playerKeys[index][3]))
                ship.ActivatePrimaryWeapon(_gameTime);
            if (IsKeyNewlyDown(_playerKeys[index][4]))
                ship.ActivateSecondaryWeapon(_gameTime);

            ship.SetWantedMove(thrust, rotation);
        }
    }
}