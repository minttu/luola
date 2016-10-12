using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace luola
{
    public class InputManager
    {
        private KeyboardState _oldKeyboardState;
        private KeyboardState _keyboardState;
        private Dictionary<int, Keys[]> _playerKeys;
        private GameTime _gameTime;

        public InputManager()
        {
            _oldKeyboardState = Keyboard.GetState();
            _playerKeys = new Dictionary<int, Keys[]>
            {
                [0] = new Keys[] {Keys.Up, Keys.Left, Keys.Right, Keys.RightShift, Keys.Down},
                [1] = new Keys[] {Keys.W, Keys.A, Keys.D, Keys.LeftShift, Keys.S}
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

            if (this.IsKeyDown(_playerKeys[index][0]))
                thrust = 1f;
            if (this.IsKeyDown(_playerKeys[index][1]))
                rotation = -1f;
            if (this.IsKeyDown(_playerKeys[index][2]))
                rotation = 1f;
            if (this.IsKeyNewlyDown(_playerKeys[index][3]))
                ship.ActivatePrimaryWeapon(_gameTime);
            if (this.IsKeyNewlyDown(_playerKeys[index][4]))
                ship.ActivateSecondaryWeapon(_gameTime);

            ship.SetWantedMove(thrust, rotation);
        }
        
    }
}