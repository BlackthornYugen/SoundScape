using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib.Scenes;


namespace SoundScape
{
    /// <summary>
    /// The info scene is used to display a single image to the user. 
    /// The “Help” and “How To Play” pages are built using this class. 
    /// Also the HighScoreScene extends it. 
    /// </summary>
    public class InfoScene : GameScene
    {
        private readonly Texture2D _texture;
        protected Vector2 _centerScreen;

        public InfoScene(GameLoop game, Texture2D texture, Texture2D background, Vector2 centerScreen)
            : base(game, game.SpriteBatch)
        {
            _texture = texture;
            _background = background;
            _centerScreen = centerScreen;
        }

        public new GameLoop Game
        {
            get { return base.Game as GameLoop; }
        }

        protected Texture2D Texture
        {
            get { return _texture; }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spritebatch.Begin();
            _spritebatch.Draw(_texture, _centerScreen, Color.White);
            _spritebatch.End();
        }
    }
}
