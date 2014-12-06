using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    }
}
