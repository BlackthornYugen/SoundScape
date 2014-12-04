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
            set
            {
                 _score = value;

            }
        }

        public HighScore(GameLoop game, Texture2D texture, Texture2D background, Vector2 centerScreen,
            int score)
            : base(game, texture, background, centerScreen)
        {
            _score = score;
        }

        public void updateScore(int score)
        {
            if (score > _score)
            {
                _score = score;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteFont regularFont = Game.Content.Load<SpriteFont>("fonts/regularFont");

            string msg = "";
            _spritebatch.Begin();
            
            _spritebatch.Draw(Texture, _centerScreen, Color.White);
            msg = _score.ToString();

            _spritebatch.DrawString(regularFont, msg, _centerScreen + new Vector2(60, 90), Color.CornflowerBlue, 0,
                Vector2.Zero, 1f, SpriteEffects.None, 0);
            _spritebatch.End();
        }
    }
}