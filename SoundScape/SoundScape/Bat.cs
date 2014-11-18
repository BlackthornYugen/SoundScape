using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SoundScape
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Bat : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Vector2 position;
        private Vector2 initPosition;
        private Vector2 speed;


        public Bat(Game game, Texture2D texture)
            : base(game)
        {
            this.spriteBatch = (game as Game1).SpriteBatch;
            this.texture = texture;
            this.initPosition = new Vector2(0, Game.Window.ClientBounds.Height - texture.Height);
            this.position = this.initPosition;
            this.speed = new Vector2(4, 0);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Right))
                position += speed;
            if (ks.IsKeyDown(Keys.Left))
                position -= speed;



            if (position.X < 0)
                position.X = 0;
            else if (position.X > Game.Window.ClientBounds.Width - texture.Width)
                position.X = Game.Window.ClientBounds.Width - texture.Width;

            // Copy me!!!

            // Copy me too?

            /*
             * 
             * Copy us
             * copy us
             * CoPy Us
             * 
             * 
             */ 

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
