using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundScape
{
    class VirtualController : GameComponent
    {
        private PlayerIndex _playerIndex;
        private GamePadState _padState;
        private KeyboardState _keyState;
        private MouseState _ratState;

        public VirtualController(Game game, PlayerIndex playerIndex) : base(game)
        {
            _playerIndex = playerIndex;
        }

        public override void Update(GameTime gameTime)
        {
            _padState = GamePad.GetState(_playerIndex);
            _keyState = Keyboard.GetState();
            _ratState = Mouse.GetState();
            base.Update(gameTime);
        }

        public bool ActionMenuUp
        {
            get
            {
                Keys[] keys = { Keys.Up, Keys.OemPlus, Keys.PageUp };
                Buttons[] button = { Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp };
                return keys.Any(key => _keyState.IsKeyDown(key));
            }
        }

        public bool ActionMenuDown
        {
            get
            {
                Keys[] keys = { Keys.Down, Keys.OemMinus, Keys.PageDown };
                Buttons[] button = { Buttons.DPadDown, Buttons.LeftThumbstickDown, Buttons.RightThumbstickUp };
                return KeyPressed(keys) || ButtonPressed(button);
            }
        }

        public bool KeyPressed(Keys key, KeyboardState? ks = null)
        {
            ks = ks ?? Keyboard.GetState();
            return ks.Value.IsKeyDown(key) && ks.Value.IsKeyUp(key);
        }

        public bool KeyPressed(Keys[] keys, KeyboardState? ks = null)
        {
            ks = ks ?? Keyboard.GetState();
            return keys.Any(key => ks.Value.IsKeyDown(key) && ks.Value.IsKeyUp(key));
        }

        public bool KeyDown(Keys key)
        {
            return false;
        }

        public bool ButtonPressed(Buttons button)
        {
            return false;
        }

        public bool ButtonPressed(Buttons[] button)
        {
            return false;
        }

        public bool ButtonDown(Buttons button)
        {
            return false;
        }
    }
}
