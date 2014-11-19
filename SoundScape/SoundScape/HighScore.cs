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
using XNALib.Scenes;


namespace SoundScape
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class HighScore : HelpScene
    {
        List< int> highScores;

        public List<int> HighScores
        {
            get { return highScores; }
            set
            {
                if (highScores.Count < 10)
                {
                    highScores = value;
                }
            }
        }

        public HighScore(Game game, Texture2D tex, List<int> highScores)
            : base(game, tex)
        {
            this.highScores = highScores;
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
            // TODO: Add your update code here
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteFont regularFont = Game.Content.Load<SpriteFont>("fonts/regularFont");

            string msg = "";
            spritebatch.Begin();

            spritebatch.Draw(texture, Vector2.Zero, Color.White);

            for (int i = 0; i < highScores.Count; i++)
            {
                msg += highScores[i] + " \n";
            }

            spritebatch.DrawString(regularFont, msg, new Vector2(60, 90), Color.CornflowerBlue, 0,
                new Vector2(), 1f, SpriteEffects.None, 0);

            spritebatch.End();
        }
    }
}
