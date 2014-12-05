using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoundScape.GameplaySceneComponents
{
    /// <summary>
    ///     A player object
    /// </summary>
    internal class Player : GameplaySceneComponent
    {
        private const int DISTANCE_FACTOR = 30;
        private const float PITCH_VELOCITY = 0.01f;
        private const float TOLLERENCE = 0.01f;
        private const int SCORE = -50;
        private readonly SoundEffectInstance _weaponSoundEffectInstance;
        private SoundEffectInstance _activeSound;

        private Vector2[] _aimVectors;
        private Vector2 _arrow;
        private VirtualController _controller;
        private float _pan;
        private float _rumbleLeft;
        private DateTime _rumbleLeftTime;
        private float _rumbleRight;
        private DateTime _rumbleRightTime;
        private WeaponState _weaponState = WeaponState.Discharged;

        /// <summary>
        ///     Construct a player with a specific colour
        /// </summary>
        /// <param name="scene">
        ///     A reference to the scene that the player
        ///     will inhabit.
        /// </param>
        /// <param name="spriteBatch">
        ///     A reference to a spritebatch
        ///     object.
        /// </param>
        /// <param name="position">
        ///     The starting position of the player.
        /// </param>
        /// <param name="texture">The player's texture.</param>
        /// <param name="soundEffect">
        ///     The soundeffect that identifies this
        ///     player.
        /// </param>
        /// <param name="pan">
        ///     The audio channel that sound is played on for
        ///     this player where -1 is full left and 1 is full right.
        /// </param>
        /// <param name="weaponSoundEffect">
        ///     The sound used for the player's
        ///     weapon.
        /// </param>
        /// <param name="colour">The colour to draw the player.</param>
        public Player(GameplayScene scene, SpriteBatch spriteBatch, Vector2 position, Texture2D texture,
            SoundEffect soundEffect, float pan, SoundEffect weaponSoundEffect, Color colour)
            : base(scene, spriteBatch, position, texture, soundEffect, colour)
        {
            _pan = pan;
            _weaponSoundEffectInstance = weaponSoundEffect.CreateInstance();
            _weaponSoundEffectInstance.Pan = pan;
            _weaponSoundEffectInstance.Pitch = -1;
        }

        /// <summary>
        ///     The left motor's rumble percentage.
        /// </summary>
        public float RumbleLeft
        {
            get { return _rumbleLeft; }
            set
            {
                if (Math.Abs(_rumbleLeft - value) > TOLLERENCE)
                {
                    _rumbleLeft = value;
                    GamePad.SetVibration(Controller.PlayerIndex, _rumbleLeft, _rumbleRight);
                }
            }
        }

        /// <summary>
        ///     The right motor's rumble percentage.
        /// </summary>
        public float RumbleRight
        {
            get { return _rumbleRight; }
            set
            {
                if (Math.Abs(_rumbleRight - value) > TOLLERENCE)
                {
                    _rumbleRight = value;
                    GamePad.SetVibration(Controller.PlayerIndex, _rumbleLeft, _rumbleRight);
                }
            }
        }


        /// <summary>
        ///     Control the audio channel that sound is played on for this player.
        /// </summary>
        public float Pan
        {
            get { return _pan; }
            set { _pan = value; }
        }

        public override int Score
        {
            get { return SCORE; }
        }

        public Texture2D SonarTexture { get; set; }

        public VirtualController Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        /// <summary>
        ///     Update movement, handle collision and handle rumbling
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Toggle Visibility
            if (Controller.ButtonPressed(Buttons.Y))
            {
                Scene.Visible = !Scene.Visible;
            }

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
            float deltaX = Controller.MovementAxisX;
            float deltaY = -Controller.MovementAxisY;
            Vector2 oldPosition = Position;
            Position += new Vector2(deltaX*3, deltaY*3);

            foreach (IGameComponent component in Scene.Components)
            {
                var gsc = component as GameplaySceneComponent;
                if (gsc != this && gsc != null)
                {
                    // TODO: Stop ignoring collision when right sholder is pressed. 
                    if (!Controller.ButtonDown(Buttons.A)
                        && gsc.Enabled
                        && gsc.Hitbox.Intersects(Hitbox))
                    {
                        // TODO: Remove godmode when left sholder is pressed. 
                        if (gsc is Enemy) Kill(Colour);
                        else
                        {
                            Position = oldPosition;
                            // Rumble for the player coliding.
                            RumbleFor(75, 1);
                            if (gsc is Player)
                            {
                                // Rumble for other player
                                (gsc as Player).RumbleFor(50, rightMotor: 1);
                            }
                        }
                    }
                }
            }

            // Weapon
            if (_weaponState == WeaponState.Charging)
            {
                if (_weaponSoundEffectInstance.State == SoundState.Stopped)
                    _weaponState = WeaponState.Charged;
                else
                    _weaponSoundEffectInstance.Pitch = Math.Min(_weaponSoundEffectInstance.Pitch + PITCH_VELOCITY, 1);
            }

            if (_weaponState == WeaponState.Cooldown)
            {
                if (_weaponSoundEffectInstance.Pitch < 0)
                {
                    _weaponState = WeaponState.Discharged;
                }
                else
                {
                    _weaponSoundEffectInstance.Pitch = Math.Max(_weaponSoundEffectInstance.Pitch - PITCH_VELOCITY, -1);
                }
            }

            else if (_weaponState == WeaponState.Discharged && Controller.ActionFire)
            {
                _weaponSoundEffectInstance.Pitch = -1;
                _weaponSoundEffectInstance.Play();
                _weaponState = WeaponState.Charging;
                Colour = new Color(Colour.R/2, Colour.G/2, Colour.B/2);
            }

            // Sound Wave
            _arrow = new Vector2(_controller.AimAxisX, -_controller.AimAxisY)*DISTANCE_FACTOR;

            bool hitSomething = false;

            var arrowLength = (int) _arrow.Length();

            if (_weaponState == WeaponState.Charged ||
                _weaponState == WeaponState.Charging)
                // Shots fired go DISTANCE_FACTOR further than sonar.
                arrowLength *= DISTANCE_FACTOR;

            _aimVectors = new Vector2[arrowLength];

            for (int i = 0; i < arrowLength && !hitSomething; i++)
            {
                _aimVectors[i] = Position + _arrow*i;
                foreach (IGameComponent gameComponent in Scene.Components)
                {
                    var gsc = gameComponent as GameplaySceneComponent;
                    if (gsc != this && gsc != null && gsc.Enabled)
                    {
                        hitSomething = HitSomething(gsc, i, arrowLength);
                    }
                }
            }

            base.Update(gameTime);
        }

        private bool HitSomething(GameplaySceneComponent gsc, int i, int limit)
        {
            bool hitSomething = false;
            if (gsc.Hitbox.Contains((int) _aimVectors[i].X, (int) _aimVectors[i].Y))
            {
                if (_weaponState == WeaponState.Charged ||
                    _weaponState == WeaponState.Charging)
                {
                    _weaponSoundEffectInstance.Pitch = 1;
                    _weaponSoundEffectInstance.Play();
                    _weaponState = WeaponState.Cooldown;
                    Colour = new Color(Colour.R*2, Colour.G*2, Colour.B*2);
                    gsc.Kill(Colour);
                }
                else if (_weaponState != WeaponState.Charging &&
                         _weaponState != WeaponState.Cooldown)
                {
                    hitSomething = true;
                    if (_activeSound == null || _activeSound.State == SoundState.Stopped)
                    {
                        SonarHit(gsc, 1f - (float) i/limit);
                    }
                }
            }
            return hitSomething;
        }

        private void SonarHit(GameplaySceneComponent gsc, float volume)
        {
            _activeSound = gsc.SoundEffect.CreateInstance();
            _activeSound.Volume = volume;
            _activeSound.Pan = _pan;
            _activeSound.Play();
        }

        /// <summary>
        ///     Draw the player at their current location.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            Color vColor = Colour;
            SpriteBatch.Begin();
            if (_aimVectors != null)
                for (int i = 5; i < _aimVectors.Length; i++)
                {
                    Vector2 aimVector = _aimVectors[i];
                    SpriteBatch.Draw(SonarTexture,
                        aimVector - new Vector2(SonarTexture.Width/2f, SonarTexture.Height/2f), vColor);
                    vColor.R = (byte) (vColor.R/1.1);
                    vColor.G = (byte) (vColor.G/1.1);
                    vColor.B = (byte) (vColor.B/1.1);
                }
            SpriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        ///     Cause the controller to rumble for a specified time.
        /// </summary>
        /// <param name="miliseconds">
        ///     The time in miliseconds to rumble
        ///     the player's remote for.
        /// </param>
        /// <param name="leftMotor">
        ///     The percentage of power to use for
        ///     the left motor.
        /// </param>
        /// <param name="rightMotor">
        ///     The percentage of power to use for
        ///     the right motor.
        /// </param>
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

            if (Enabled)
            {
                GamePad.SetVibration(Controller.PlayerIndex, _rumbleLeft, _rumbleRight);
            }
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            _rumbleLeft = 0;
            _rumbleRight = 0;
            GamePad.SetVibration(Controller.PlayerIndex, 0, 0);
            base.OnEnabledChanged(sender, args);
        }

        public override void Kill(Color? killedByColour = null)
        {
            if (Controller.ButtonDown(Buttons.LeftShoulder))
                RumbleFor(75, 1);
            else
                base.Kill(killedByColour);
        }

        private enum WeaponState
        {
            Charged,
            Charging,
            Discharged,
            Cooldown
        }
    }
}