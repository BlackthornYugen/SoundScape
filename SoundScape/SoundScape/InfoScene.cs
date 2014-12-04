using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib.Scenes;


namespace SoundScape
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InfoScene : GameScene
    {
        //TODO: background
        protected Texture2D _backGround;


        private readonly Texture2D _texture;
        public InfoScene(GameLoop game, Texture2D texture, Texture2D backGround)
            : base(game, game.SpriteBatch)
        {
            _texture = texture;
            _backGround = backGround;
        }

        protected Texture2D Texture
        {
            get { return _texture; }
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
            _spritebatch.Begin();
            if (_backGround != null)
            {
                _spritebatch.Draw(_backGround, new Vector2(), Color.White);
            }
            _spritebatch.Draw(_texture, Vector2.Zero,  Color.White);
            _spritebatch.End();
            base.Draw(gameTime);
        }
    }
}
