using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoundScape.GameplaySceneComponents
{
    class Player : GameplaySceneComponent
    {
        private const int DISTANCE_FACTOR = 30;
        private PlayerIndex _controllerIndex;
        private GamePadState _padState;
        private GamePadState _padOldState;
        private Vector2 _arrow;
        private Vector2[] _aimVectors;
        private SoundEffectInstance _activeSound;
        private float _rumbleLeft, _rumbleRight;
        private DateTime _rumbleLeftTime, _rumbleRightTime;

        public Player(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, 
            SoundEffect soundEffect, Rectangle hitbox)
            : base(scene, spriteBatch, position, texture, soundEffect, hitbox)
        {
        }

        public Player(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, 
            SoundEffect soundEffect, Rectangle hitbox, Color colour)
            : base(scene, spriteBatch, position, texture, soundEffect, hitbox, colour)
        {
        }

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
                        Position = oldPosition;
                        // Rumble for the player coliding.
                        RumbleFor(75, 1);
                        if (gsc is Player)
                        {   // Rumble for other player
                            (gsc as Player).RumbleFor(50, -1, 1);
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
                                // TODO: Rework distance formula to be more reliable
                                var distance = (gsc.Position - Position).Length() / (Position - (Position + _arrow * limit)).Length();
                                distance = Math.Min(Math.Max(1f - distance, 0f), 1f); 
                                _activeSound = gsc.SoundEffect.CreateInstance();
                                _activeSound.Volume = distance;
                                _activeSound.Play();
                            }
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            foreach (Vector2 aimVector in _aimVectors)
            {
                if (aimVector != Vector2.Zero)
                {
                    SpriteBatch.Draw(Texture, aimVector, Colour);
                }
            }
            SpriteBatch.End();
            base.Draw(gameTime);
        }


        public PlayerIndex ControllerIndex
        {
            get { return _controllerIndex; }
            set { _controllerIndex = value; }
        }

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
