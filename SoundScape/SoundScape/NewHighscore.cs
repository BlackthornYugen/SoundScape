using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundScape.Levels;
using XNALib.Menus;
using XNALib.Scenes;

namespace SoundScape
{
    class NewHighscore : GameScene
    {
        private Color _regularColour;
        private Color _highlightColor;
        private SpriteFont _font;
        const int MAX_LETTERS = 15;
        private MenuComponent<char>[] _letterMenuComponents;

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
            _letterMenuComponents = new MenuComponent<char>[MAX_LETTERS];
            Vector2 startingPos = Vector2.One*100;
            for (int i = 0; i < MAX_LETTERS; i++)
            {
                _letterMenuComponents[i] = new MenuComponent<char>(Game, _spritebatch, _regularColour, _highlightColor, _font, _font,
                    startingPos + Vector2.UnitX * 75 * i) { Logo = Game.Content.Load<Texture2D>("logo"), LogoPosition = Vector2.Zero };
                _letterMenuComponents[i].Add(" ", ' ');
                for (char c = 'A'; c < 'Z'; c++)
                {
                    _letterMenuComponents[i].Add(c.ToString(), c);
                }
                Components.Add(_letterMenuComponents[i]);
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Game.PlayerOne.ActionSelect || Game.PlayerTwo.ActionSelect)
            {
                
            }
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
