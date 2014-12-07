using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib.Scenes;

namespace SoundScape
{
    class ControllerStatus : GameScene
    {
        private const byte COLOUR_INTENSITY = 255;

        private struct ControllerData
        {
            public static Texture2D ConnectedTexture;
            public static Texture2D DisconnectedTexture;
            public const int Scale = 1;
            public bool Connected;
            public Color Colour;
            public Vector2 Position;

            public Texture2D Texture
            {
                get
                {
                    return Connected ? ConnectedTexture : DisconnectedTexture;
                }
            }

            public int TextureWidth
            {
                get
                {
                    return (Connected ? ConnectedTexture.Width : DisconnectedTexture.Width) * Scale;
                }
            }

            public int TextureHeight
            {
                get
                {
                    return (Connected ? ConnectedTexture.Height : DisconnectedTexture.Height) * Scale;
                }
            }
        }


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
            ControllerData.ConnectedTexture = Game.Content.Load<Texture2D>("images/controller/connected");
            ControllerData.DisconnectedTexture = Game.Content.Load<Texture2D>("images/controller/disconnected");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 marginY = Vector2.UnitY * MarginY;
            Vector2 marginX = Vector2.UnitX * MarginX;
            Vector2 windowWidth = Vector2.UnitX * (Game.Window.ClientBounds.Width - ControllerData.ConnectedTexture.Width * ControllerData.Scale) - marginX * 2;
            Vector2 windowHeight = Vector2.UnitY * (Game.Window.ClientBounds.Height - ControllerData.ConnectedTexture.Height * ControllerData.Scale);
            _spritebatch.Begin();
            foreach (var player in new []
            {
                new ControllerData { 
                    Connected = Game.PlayerOne.Connected, 
                    Colour = new Color(COLOUR_INTENSITY,COLOUR_INTENSITY/2,COLOUR_INTENSITY/2,255),
                    Position = marginX - marginY + windowHeight}, 
                new ControllerData { 
                    Connected = Game.PlayerTwo.Connected, 
                    Colour = new Color(COLOUR_INTENSITY/2,COLOUR_INTENSITY/2,COLOUR_INTENSITY,255),
                    Position = marginX - marginY + windowWidth + windowHeight}
            })
            {
                _spritebatch.Draw(player.Texture,
                    new Rectangle((int)player.Position.X, (int)player.Position.Y, player.TextureWidth, player.TextureHeight), player.Colour);
            }
            _spritebatch.End();           
            base.Draw(gameTime);
        }
    }
}
