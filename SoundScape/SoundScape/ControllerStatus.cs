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
        private int _textureHeight;
        public int MarginY { get; set; }
        public int MarginX { get; set; }

        public new GameLoop Game
        {
            get { return base.Game as GameLoop; }
        }
        public ControllerStatus(Game game, SpriteBatch spritebatch) : base(game, spritebatch)
        {
            MarginY = 100;
            MarginX = 100;
        }

        protected override void LoadContent()
        {
            _connectedTexture = Game.Content.Load<Texture2D>("images/controller/connected");
            _disconnectedTexture = Game.Content.Load<Texture2D>("images/controller/disconnected");
            _textureWidth = (_connectedTexture.Width + _disconnectedTexture.Width) / 2;
            _textureHeight = (_connectedTexture.Height + _disconnectedTexture.Height) / 2;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 marginY = Vector2.UnitY * MarginY;
            Vector2 marginX = Vector2.UnitX*MarginX;
            Vector2 windowWidth = Vector2.UnitX * (Game.Window.ClientBounds.Width - _textureWidth) - marginX*2;
            Vector2 windowHeight = Vector2.UnitY * (Game.Window.ClientBounds.Height - _textureHeight);
            _spritebatch.Begin();
            foreach (var player in new []
            {
                new ControllerData { 
                    Connected = Game.PlayerOne.Connected, 
                    Colour = Color.Red,
                    Position = marginX - marginY + windowHeight}, 
                new ControllerData { 
                    Connected = Game.PlayerTwo.Connected, 
                    Colour = Color.Blue,
                    Position = marginX - marginY + windowWidth + windowHeight}
            })
            {
                Texture2D texture = player.Connected ? _connectedTexture : _disconnectedTexture;

                _spritebatch.Draw(texture, player.Position, player.Colour);
            }
            _spritebatch.End();           
            base.Draw(gameTime);
        }

        private struct ControllerData
        {
            public bool Connected;
            public Color Colour;
            public Vector2 Position;
        }
    }
}
