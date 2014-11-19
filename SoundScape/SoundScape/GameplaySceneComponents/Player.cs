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
        private PlayerIndex controllerIndex;
        private GamePadState padState;
        private GamePadState padOldState;


        public Player(Game game, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, 
            SoundEffect soundEffect, Rectangle hitbox)
            : base(game, spriteBatch, position, texture, soundEffect, hitbox)
        {
        }

        public Player(Game game, SpriteBatch spriteBatch, Vector2 position, Texture2D texture, 
            SoundEffect soundEffect, Rectangle hitbox, Color colour)
            : base(game, spriteBatch, position, texture, soundEffect, hitbox, colour)
        {
        }

        public override void Update(GameTime gameTime)
        {
            padState = GamePad.GetState(ControllerIndex);
            float deltaX = padState.ThumbSticks.Left.X;
            float deltaY = -padState.ThumbSticks.Left.Y;

            if (padState.IsConnected &&
                padState.DPad.Down == ButtonState.Pressed &&
                padState.DPad.Down == ButtonState.Released)
                Console.WriteLine(padState.DPad.Down);

            Position += new Vector2(deltaX*3, deltaY*3);
            padOldState = padState;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var arrow = new Vector2(padState.ThumbSticks.Right.X, -padState.ThumbSticks.Right.Y) * 100;
            //if (!arrow.Equals(Vector2.Zero))
            {

                SpriteBatch.Begin();
                for (int i = 0; i < arrow.Length(); i++)
                {
                    SpriteBatch.Draw(Texture, Position + arrow*i, Colour);
                }
                SpriteBatch.End();
            }
            base.Draw(gameTime);
        }


        public PlayerIndex ControllerIndex
        {
            get { return controllerIndex; }
            set { controllerIndex = value; }
        }
    }
}
