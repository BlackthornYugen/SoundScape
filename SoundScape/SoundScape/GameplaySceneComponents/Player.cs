using System;
using System.Collections.Generic;
using System.Linq;
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
            
            // Movement Controlls
            float deltaX = _padState.ThumbSticks.Left.X;
            float deltaY = -_padState.ThumbSticks.Left.Y;

            if (_padState.IsConnected &&
                _padState.DPad.Down == ButtonState.Pressed &&
                _padState.DPad.Down == ButtonState.Released)
                Console.WriteLine(_padState.DPad.Down);

            Position += new Vector2(deltaX*3, deltaY*3);
            _padOldState = _padState;

            // Aiming Controlls
            _arrow = new Vector2(_padState.ThumbSticks.Right.X, -_padState.ThumbSticks.Right.Y) * DISTANCE_FACTOR;
            _aimVectors = new Vector2[(int)_arrow.Length()];

            bool flag = true;
            var limit = (int) _arrow.Length();

            for (int i = 0; i < limit && flag; i++)
            {
                _aimVectors[i] = Position + _arrow * i;
                foreach (IGameComponent gameComponent in Scene.Components)
                {
                    if (gameComponent != this && gameComponent is GameplaySceneComponent)
                    {
                        var gsc = gameComponent as GameplaySceneComponent;
                        if (gsc.Hitbox.Contains((int) _aimVectors[i].X, (int) _aimVectors[i].Y))
                        {
                            flag = false;
                            if (_activeSound == null || _activeSound.State == SoundState.Stopped)
                            {
                                // TODO: Rework distance formula to be more reliable
                                var distance = (gsc.Position - Position).Length() / (Position - (Position + _arrow * limit)).Length();
                                distance = Math.Min(Math.Max(1f - distance, 0f), 1f); 
                                Console.WriteLine(gsc.GetType());
                                if (gsc is Player)
                                    Console.WriteLine(((Player)gsc).ControllerIndex);
                                _activeSound = gsc.SoundEffect.CreateInstance();
                                _activeSound.Volume = distance;
                                _activeSound.Play();
                                Scene.Game.Window.Title = distance.ToString();
                            }
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //if (!arrow.Equals(Vector2.Zero))
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
            }
            base.Draw(gameTime);
        }


        public PlayerIndex ControllerIndex
        {
            get { return _controllerIndex; }
            set { _controllerIndex = value; }
        }
    }
}
