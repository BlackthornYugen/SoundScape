using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib.Scenes;

namespace SoundScape
{
    class ControllerStatus : GameScene
    {
        private Texture2D _connectedTexture;
        private Texture2D _disconnectedTexture;
        private int _textureWidth;

        public new GameLoop Game
        {
            get { return base.Game as GameLoop; }
        }
        public ControllerStatus(Game game, SpriteBatch spritebatch) : base(game, spritebatch)
        {
        }

        protected override void LoadContent()
        {
            _connectedTexture = Game.Content.Load<Texture2D>("images/controller/connected");
            _disconnectedTexture = Game.Content.Load<Texture2D>("images/controller/disconnected");
            _textureWidth = (_connectedTexture.Width + _disconnectedTexture.Width)/2;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            int windowWidth = Game.Window.ClientBounds.Width;
            _spritebatch.Begin();
            _spritebatch.Draw(Game.PlayerOne.Connected ? _connectedTexture : _disconnectedTexture, Vector2.Zero, Color.Red);
            _spritebatch.Draw(Game.PlayerTwo.Connected ? _connectedTexture : _disconnectedTexture, Vector2.UnitX * (windowWidth - _textureWidth), Color.Blue);
            _spritebatch.End();           
            base.Draw(gameTime);
        }
    }
}
