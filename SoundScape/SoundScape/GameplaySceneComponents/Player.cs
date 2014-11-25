using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoundScape.GameplaySceneComponents
{
    /// <summary>
    /// A player object
    /// </summary>
    class Player : GameplaySceneComponent
    {
        private const int DISTANCE_FACTOR = 30;
        private PlayerIndex _controllerIndex;
        private GamePadState _padState;
        private GamePadState _padOldState;
        private Vector2 _arrow;
        private Vector2[] _aimVectors;
        private float _pan;
        private SoundEffectInstance _activeSound;
        private float _rumbleLeft, _rumbleRight;
        private DateTime _rumbleLeftTime, _rumbleRightTime;

        /// <summary>
        /// Construct a player
        /// </summary>
        /// <param name="scene">A reference to the scene that the player 
        /// will inhabit.</param>
        /// <param name="spriteBatch">A reference to a spritebatch 
        /// object.</param>
        /// <param name="position">The starting position of the player.
        /// </param>
        /// <param name="texture">The player's texture.</param>
        /// <param name="soundEffect">The soundeffect that identifies this 
        /// player.</param>
        /// <param name="pan">The audio channel that sound is played on for 
        /// this player where -1 is full left and 1 is full right.</param>
        public Player(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, 
            SoundEffect soundEffect, float pan)
            : base(scene, spriteBatch, position, texture, soundEffect)
        {
            _pan = pan;
        }

        /// <summary>
        /// Construct a player with a specific colour
        /// </summary>
        /// <param name="scene">A reference to the scene that the player 
        /// will inhabit.</param>
        /// <param name="spriteBatch">A reference to a spritebatch 
        /// object.</param>
        /// <param name="position">The starting position of the player.
        /// </param>
        /// <param name="texture">The player's texture.</param>
        /// <param name="soundEffect">The soundeffect that identifies this 
        /// player.</param>
        /// <param name="pan">The audio channel that sound is played on for 
        /// this player where -1 is full left and 1 is full right.</param>
        /// <param name="colour">The colour to draw the player.</param>
        public Player(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect, float pan, Color colour)
            : base(scene, spriteBatch, position, texture, soundEffect, colour)
        {
            _pan = pan;
        }

        /// <summary>
        /// Update movement, handle collision and handle rumbling
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _padState = GamePad.GetState(ControllerIndex);

            // Expire rumbles
            if (_rumbleLeftTime < DateTime.Now)
            {
                RumbleLeft = 0;
                _rumbleLeftTime = DateTime.MaxValue;
            }

            if (_rumbleRightTime < DateTime.Now)
            {
                RumbleRight = 0;
                _rumbleRightTime = DateTime.MaxValue;
            }

            // Movement
            float deltaX = _padState.ThumbSticks.Left.X;
            float deltaY = -_padState.ThumbSticks.Left.Y;
            var oldPosition = Position;
            Position += new Vector2(deltaX*3, deltaY*3);
            
            foreach (IGameComponent component in Scene.Components)
            {
                if (component != this && component is GameplaySceneComponent)
                {
                    GameplaySceneComponent gsc = component as GameplaySceneComponent;
                    // TODO: Stop ignoring collision when right sholder is pressed. 
                    if (_padState.Buttons.RightShoulder == ButtonState.Released 
                        && gsc.Hitbox.Intersects(this.Hitbox))
                    {
                        if (gsc is Enemy)
                        {
                            this.Visible = false;
                            this.Enabled = false;
                            //this.RumbleFor(3000, 1, 1);
                        }
                        else
                        {
                            Position = oldPosition;
                            // Rumble for the player coliding.
                            RumbleFor(miliseconds: 75, leftMotor: 1);
                            if (gsc is Player)
                            {   // Rumble for other player
                                (gsc as Player).RumbleFor(miliseconds: 50, rightMotor: 1);
                            }
                        }
                    }
                }
            }

            _padOldState = _padState;

            // Sound Wave
            _arrow = new Vector2(_padState.ThumbSticks.Right.X, -_padState.ThumbSticks.Right.Y) * DISTANCE_FACTOR;
            _aimVectors = new Vector2[(int)_arrow.Length()];

            bool soundReflected = false;
            int limit = (int) _arrow.Length();

            for (int i = 0; i < limit && !soundReflected; i++)
            {
                _aimVectors[i] = Position + _arrow * i;
                foreach (IGameComponent gameComponent in Scene.Components)
                {
                    if (gameComponent != this && gameComponent is GameplaySceneComponent)
                    {
                        var gsc = gameComponent as GameplaySceneComponent;
                        if (gsc.Hitbox.Contains((int) _aimVectors[i].X, (int) _aimVectors[i].Y))
                        {
                            soundReflected = true;
                            if (_activeSound == null || _activeSound.State == SoundState.Stopped)
                            {
                                _activeSound = gsc.SoundEffect.CreateInstance();
                                _activeSound.Volume = 1f - (float)i / limit;
                                _activeSound.Pan = _pan;
                                _activeSound.Play();
                            }
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the player at their current location.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            foreach (Vector2 aimVector in _aimVectors)
            {
                if (aimVector != Vector2.Zero)
                {
                    SpriteBatch.Draw(Texture, aimVector - new Vector2(Texture.Width / 2f, Texture.Height / 2f), Colour);
                }
            }
            SpriteBatch.End();
            base.Draw(gameTime);
        }


        /// <summary>
        /// The controler used for this player.
        /// </summary>
        public PlayerIndex ControllerIndex
        {
            get { return _controllerIndex; }
            set { _controllerIndex = value; }
        }

        /// <summary>
        /// The left motor's rumble percentage.
        /// </summary>
        public float RumbleLeft
        {
            get { return _rumbleLeft; }
            set
            {
                if (Math.Abs(_rumbleLeft - value) > 0.01)
                {
                    _rumbleLeft = value;
                    GamePad.SetVibration(_controllerIndex, _rumbleLeft, _rumbleRight);
                }
            }
        }

        /// <summary>
        /// The right motor's rumble percentage.
        /// </summary>
        public float RumbleRight
        {
            get { return _rumbleRight; }
            set
            {
                if (Math.Abs(_rumbleRight - value) > 0.01)
                {
                    _rumbleRight = value;
                    GamePad.SetVibration(_controllerIndex, _rumbleLeft, _rumbleRight);
                }
            }
        }


        /// <summary>
        /// Control the audio channel that sound is played on for this player.
        /// </summary>
        public float Pan
        {
            get { return _pan; }
            set { _pan = value; }
        }

        /// <summary>
        /// Cause the controller to rumble for a specified time.
        /// </summary>
        /// <param name="miliseconds">The time in miliseconds to rumble
        /// the player's remote for.</param>
        /// <param name="leftMotor">The percentage of power to use for
        /// the left motor.</param>
        /// <param name="rightMotor">The percentage of power to use for 
        /// the right motor.</param>
        public void RumbleFor(int miliseconds, float leftMotor = -1f, float rightMotor = -1f)
        {
            if (leftMotor > 0)
            {
                _rumbleLeftTime = DateTime.Now.AddMilliseconds(miliseconds);
                _rumbleLeft = leftMotor;
            }

            if (rightMotor > 0)
            {
                _rumbleRightTime = DateTime.Now.AddMilliseconds(miliseconds);
                _rumbleRight = rightMotor;                
            }

            GamePad.SetVibration(_controllerIndex, _rumbleLeft, _rumbleRight);
        }
    }
}
