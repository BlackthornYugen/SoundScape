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
        private GamePadState _padStateNew, _padStateOld;
        private KeyboardState _keyStateNew, _keyStateOld;
        private MouseState _ratStateNew, _ratStateOld;

        public VirtualController(Game game, PlayerIndex playerIndex) : base(game)
        {
            _playerIndex = playerIndex;
            _keyStateNew = Keyboard.GetState();
            _padStateNew = GamePad.GetState(PlayerIndex);
            _ratStateNew = Mouse.GetState();


            // Set Default keys
            MenuUpKeys = new[] { Keys.Up, Keys.OemPlus, Keys.PageUp };
            MenuDownKeys = new[] { Keys.Down, Keys.OemMinus, Keys.PageDown };
            MenuLeftKeys = new[] { Keys.Left };
            MenuRightKeys = new[] { Keys.Right };
            MenuSelectKeys = new[] { Keys.Enter };
            MenuBackKeys = new[] { Keys.Escape };
            MovementUpKeys = new[] { Keys.W };
            MovementDownKeys = new[] { Keys.S };
            MovementLeftKeys = new[] { Keys.A };
            MovementRightKeys = new[] { Keys.D };
            AimUpKeys = new[] { Keys.U };
            AimDownKeys = new[] { Keys.J };
            AimLeftKeys = new[] { Keys.H };
            AimRightKeys = new[] { Keys.K };
            GameFireKeys = new[] { Keys.Space };

            // Set Default buttons
            MenuSelectButtons = new[] { Buttons.Start, Buttons.A, Buttons.RightTrigger, };
            MenuBackButtons = new[] { Buttons.Back, Buttons.B };
            MenuUpButtons = new[] { Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp };
            MenuDownButtons = new[] { Buttons.DPadDown, Buttons.LeftThumbstickDown, Buttons.RightThumbstickDown };
            MenuLeftButtons = new[] { Buttons.DPadLeft, Buttons.LeftThumbstickLeft, Buttons.RightThumbstickLeft };
            MenuRightButtons = new[] { Buttons.DPadRight, Buttons.LeftThumbstickRight, Buttons.RightThumbstickRight };
            GameFireButtons = new[] { Buttons.RightShoulder, Buttons.RightTrigger };
        }

        public override void Update(GameTime gameTime)
        {
            _padStateOld = _padStateNew;
            _keyStateOld = _keyStateNew;
            _ratStateOld = _ratStateNew;

            _padStateNew = GamePad.GetState(PlayerIndex);
            _keyStateNew = Keyboard.GetState();
            _ratStateNew = Mouse.GetState();
            base.Update(gameTime);
        }

        public bool Connected
        {
            get { return _padStateNew.IsConnected; }
        }

        public PlayerIndex PlayerIndex
        {
            get { return _playerIndex; }
        }

        public GamePadState PadState
        {
            get { return _padStateNew; }
        }

        public KeyboardState KeyStateNew
        {
            get { return _keyStateNew; }
        }

        public KeyboardState KeyStateOld
        {
            get { return _keyStateOld; }
        }

        public bool KeyPressed(IEnumerable<Keys> keys)
        {
            return keys.Any(key => _keyStateNew.IsKeyDown(key) && _keyStateOld.IsKeyUp(key));
        }

        public bool KeyDown(IEnumerable<Keys> keys)
        {
            return keys.Any(_keyStateNew.IsKeyDown);
        }

        public bool ButtonPressed(IEnumerable<Buttons> buttons)
        {
            return buttons.Any(button => _padStateNew.IsButtonDown(button) && _padStateOld.IsButtonUp(button));
        }

        public bool ButtonDown(IEnumerable<Buttons> buttons)
        {
            return buttons.Any(PadState.IsButtonDown);
        }


        // Overloads for non-arrays
        internal bool ButtonPressed(Buttons button)
        {
            return ButtonPressed(new[] {button});
        }

        public bool ButtonDown(Buttons button)
        {
            return ButtonDown(new[] { button });
        }

        public bool KeyPressed(Keys key)
        {
            return KeyPressed(new[] { key });
        }

    }
}
