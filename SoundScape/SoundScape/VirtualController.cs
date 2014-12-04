using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundScape
{
    public partial class VirtualController : GameComponent
    {
        private PlayerIndex _playerIndex;
        private GamePadState _padState;
        private KeyboardState _keyState;
        private MouseState _ratState;

        public VirtualController(Game game, PlayerIndex playerIndex) : base(game)
        {
            _playerIndex = playerIndex;
            _padState = GamePad.GetState(PlayerIndex);
            _ratState = Mouse.GetState();


            // Set Default keys
            MenuUpKeys = new [] { Keys.Up, Keys.OemPlus, Keys.PageUp };
            MenuDownKeys = new [] { Keys.Down, Keys.OemMinus, Keys.PageDown };
            MenuSelectKeys = new[] { Keys.Enter };
            MenuBackKeys = new[] { Keys.Escape };
            MovementUpKeys = new[] { Keys.W };
            MovementDownKeys = new [] { Keys.S };
            MovementLeftKeys = new [] { Keys.A };
            MovementRightKeys = new [] { Keys.D };


            // Set Default buttons
            MenuSelectButtons = new [] { Buttons.Start };
            MenuBackButtons = new [] { Buttons.Start };
            MenuUpButtons = new [] {Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp};
            MenuDownButtons = new [] { Buttons.DPadDown, Buttons.LeftThumbstickDown, Buttons.RightThumbstickUp };
        }

        public override void Update(GameTime gameTime)
        {
            _padState = GamePad.GetState(PlayerIndex);
            _keyState = Keyboard.GetState();
            _ratState = Mouse.GetState();
            base.Update(gameTime);
        }

        public PlayerIndex PlayerIndex
        {
            get { return _playerIndex; }
        }

        public bool KeyPressed(Keys key, KeyboardState? ks = null)
        {
            KeyboardState keystate = ks ?? Keyboard.GetState();
            return keystate.IsKeyDown(key) && _keyState.IsKeyUp(key);
        }

        public bool KeyPressed(IEnumerable<Keys> keys, KeyboardState? ks = null)
        {
            KeyboardState keystate = ks ?? Keyboard.GetState();
            return keys.Any(key => keystate.IsKeyDown(key) && _keyState.IsKeyUp(key));
        }

        public bool KeyDown(Keys key, KeyboardState? ks = null)
        {
            KeyboardState keystate = ks ?? Keyboard.GetState();
            return keystate.IsKeyDown(key);
        }

        public bool KeyDown(IEnumerable<Keys> keys, KeyboardState? ks = null)
        {
            KeyboardState keystate = ks ?? Keyboard.GetState();
            return keys.Any(keystate.IsKeyDown);
        }

        public bool ButtonPressed(Buttons button, GamePadState? gs = null)
        {
            GamePadState padState = gs ?? GamePad.GetState(PlayerIndex);
            return padState.IsButtonDown(button) && _padState.IsButtonUp(button);
        }

        public bool ButtonPressed(IEnumerable<Buttons> buttons, GamePadState? gs = null)
        {
            GamePadState padState = gs ?? GamePad.GetState(PlayerIndex);
            return buttons.Any(button => padState.IsButtonDown(button) && _padState.IsButtonUp(button));
        }

        public bool ButtonDown(Buttons button, GamePadState? gs = null)
        {
            GamePadState padState = gs ?? GamePad.GetState(PlayerIndex);
            return padState.IsButtonDown(button);
        }

        public bool ButtonDown(IEnumerable<Buttons> buttons, GamePadState? gs = null)
        {
            GamePadState padState = gs ?? GamePad.GetState(PlayerIndex);
            return buttons.Any(padState.IsButtonDown);
        }
    }
}
