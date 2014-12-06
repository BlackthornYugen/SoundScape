using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private Color _highlightColumn;
        private Color _highlightChar;
        private SpriteFont _font;
        const int MAX_LETTERS = 15;
        private MenuComponent<char>[] _letterMenuComponents;
        private int _activePosition;

        public NewHighscore(GameLoop game, SpriteBatch sb) : base(game, sb)
        {
            _font = game.DefaultGameFont;
            _regularColour = Color.WhiteSmoke;
            _highlightColumn = Color.MediumVioletRed;
            _highlightChar = Color.Yellow;
            _activePosition = 0;
            AllowExit = false;
        }

        public new GameLoop Game
        {
            get { return base.Game as GameLoop; }
        }

        protected override void LoadContent()
        {
            _letterMenuComponents = new MenuComponent<char>[MAX_LETTERS];
            var alphabit = new char[26];
            {
                for (int i = 0; i < alphabit.Length; i++)
                {
                    alphabit[i] = (char)('A' + i);
                }   
            }

            Vector2 startingPos = Vector2.One*100;
            for (int i = 0; i < MAX_LETTERS; i++)
            {
                _letterMenuComponents[i] = new MenuComponent<char>(Game, _spritebatch, _regularColour, _highlightColumn, _font, _font,
                    startingPos + Vector2.UnitX * 75 * i)
                {
                    ColourHighlighted = _highlightColumn,
                    ColourNormal = _regularColour,
                };
                _letterMenuComponents[i].Add(" ", ' ');
                alphabit.ForEach(c => _letterMenuComponents[i].Add(c.ToString(), c));
                alphabit.ForEach(c => _letterMenuComponents[i].Add(c.ToString(), c));
                alphabit.ForEach(c => _letterMenuComponents[i].Add(c.ToString(), c));

                Components.Add(_letterMenuComponents[i]);
            }
            _letterMenuComponents.First().ColourNormal = _highlightColumn;
            _letterMenuComponents.First().ColourHighlighted = _highlightChar;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var inputs = new[] { Game.PlayerOne, Game.PlayerTwo };
            if (inputs.Any(p => p.ActionSelect))
                MoveColumn(1);
            if (inputs.Any(p => p.ActionBack))
                MoveColumn(-1);
            if (inputs.Any(p => p.ActionMenuDown))
                MoveRow(1);
            if (inputs.Any(p => p.ActionMenuUp))
                MoveRow(-1);
            base.Update(gameTime);
        }

        private void MoveColumn(int i)
        {
            Game.MenuEffects[1].Play();
            _activePosition += i;
            if (_activePosition >= _letterMenuComponents.Length || _activePosition < 0 ||
                (i > 0 && _letterMenuComponents[_activePosition - i].ActiveMenuItem.Component == ' '))
            {
                SaveScore();
            }
            else
            {
                MenuComponent<char> active = _letterMenuComponents[_activePosition];
                active.ColourNormal = _highlightColumn;
                active.ColourHighlighted = _highlightChar;
                _letterMenuComponents.Where(m => m != active).ForEach(m => m.ColourNormal = _regularColour);
            }
        }

        private void MoveRow(int i)
        {
            Game.MenuEffects[0].Play();
            var active = _letterMenuComponents[_activePosition];
            active.MenuIndex = active.MenuIndex + i;
            active.MenuIndex = Math.Max(0, active.MenuIndex);
            active.MenuIndex = Math.Min(MAX_LETTERS, active.MenuIndex);
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

        private void SaveScore()
        {
            StringBuilder name = new StringBuilder(MAX_LETTERS);
            _letterMenuComponents.ForEach(l => name.Append(l.ActiveMenuItem.Component));

            Game.HighScore.updateHighScore(name.ToString().Trim(), Campaign.CurrentScore);
            Campaign.New(); // Reset the score
            AllowExit = true;
            Game.HighScore.Show();
            Hide();
        }
    }

    internal static class Toolbox
    {
        public static void ForEach<T>(this IEnumerable<T> value, Action<T> action)
        {
            foreach (T item in value)
            {
                action(item);
            }
        }
    }
}
