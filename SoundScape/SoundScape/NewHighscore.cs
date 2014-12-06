using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.Levels;
using XNALib.Menus;
using XNALib.Scenes;

namespace SoundScape
{
    class NewHighscore : GameScene
    {
        private MenuComponent _menu;
        private Color _regularColour;
        private Color _highlightColor;
        private SpriteFont _font;

        public NewHighscore(GameLoop game, SpriteBatch sb) : base(game, sb)
        {
            _font = game.DefaultGameFont;
            _regularColour = Color.WhiteSmoke;
            _highlightColor = Color.MediumVioletRed;
        }

        public new GameLoop Game
        {
            get { return base.Game as GameLoop; }
        }

        protected override void LoadContent()
        {

            _menu = new MenuComponent(Game, _spritebatch, _regularColour, _highlightColor, _font, _font,
                Vector2.One * 100) { Logo = Game.Content.Load<Texture2D>("logo"), LogoPosition = Vector2.Zero };

            for (char c = 'A'; c < 'Z'; c++)
            {
                _menu.Add(c.ToString(), new DrawableGameComponent(Game));
            }
            Components.Add(_menu);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            var cb = Game.Window.ClientBounds;
            var font = Game.DefaultGameFont;
            string[] title = {"Congratulations", "{0:n0} points is a new record!"};
            Vector2 posVector2 = Vector2.UnitX*(cb.Width/2f);
            _spritebatch.Begin();
            foreach (string s in title)
            {
                Vector2 adjustedVector2 = posVector2 + Vector2.UnitX*(-font.MeasureString(s).X/2f);
                posVector2 += font.MeasureString(s).Y*Vector2.UnitY;
                _spritebatch.DrawString(font, string.Format(s, Campaign.CurrentScore), adjustedVector2, _regularColour);                
            }
            _spritebatch.End();
        }
    }
}
