using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SoundScape
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class HighScore : InfoScene
    {
        int _score;
        public int Score
        {
            get { return _score; }
            set{_score = value;}
        }

        public HighScore(GameLoop game, Texture2D texture, Texture2D backGround, int score)
            : base(game, texture, backGround)
        {
            _score = score;

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
            _spritebatch.Begin();
            
            //TODO: background
            if (_backGround != null)
            {
                _spritebatch.Draw(_backGround, new Vector2(), Color.White);
            }
            
            _spritebatch.Draw(Texture, Vector2.Zero, Color.White);
            msg = _score.ToString();

            _spritebatch.DrawString(regularFont, msg, new Vector2(60, 90), Color.CornflowerBlue, 0,
                new Vector2(), 1f, SpriteEffects.None, 0);
            _spritebatch.End();
        }
    }
}
