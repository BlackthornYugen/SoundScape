using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoundScape.Levels;
using XNALib.Menus;
using XNALib.Scenes;

namespace SoundScape
{
    class NewHighscoreScene : GameScene
    {
        private Color _regularColour;
        private Color _highlightColumn;
        private Color _highlightChar;
        private SpriteFont _font;
        private const char INITIAL_CHAR = 'A';
        private const int ALPHABET_LENGTH = 26;
        private const int HORAZONTAL_SPACING = 50;
        const int MAX_LETTERS = 15;
        private MenuComponent<char>[] _letterMenuComponents;
        private int _activePosition;
        private SaveStatus _saveStatus = SaveStatus.None;
        public Texture2D BannerTexture { get; set; }
        public Texture2D SavingTexture { get; set; }

        private enum SaveStatus
        {
            None,
            Requested,
            StatusDrawn,
            Saving,
            Saved,
        }
        public NewHighscoreScene(GameLoop game, SpriteBatch sb) : base(game, sb)
        {
            _font = game.DefaultGameFont;
            _regularColour = Color.WhiteSmoke;
            _highlightColumn = Color.MediumVioletRed;
            _highlightChar = Color.Yellow;
            Hide();
        }

        public new GameLoop Game
        {
            get { return base.Game as GameLoop; }
        }

        protected override void LoadContent()
        {
            _letterMenuComponents = new MenuComponent<char>[MAX_LETTERS];
            var alphabit = new char[ALPHABET_LENGTH];
            {
                for (int i = 0; i < ALPHABET_LENGTH; i++)
                {
                    alphabit[i] = (char)(INITIAL_CHAR + i);
                }   
            }

            Vector2 startingPos = Vector2.UnitX * (Game.Window.ClientBounds.Width / 2 - (HORAZONTAL_SPACING * MAX_LETTERS - HORAZONTAL_SPACING) / 2) + Vector2.UnitY * Game.Window.ClientBounds.Height / 2;
            for (int i = 0; i < MAX_LETTERS; i++)
            {
                _letterMenuComponents[i] = new MenuComponent<char>(Game, _spritebatch, _regularColour, _highlightColumn, _font, _font,
                    startingPos + Vector2.UnitX * (HORAZONTAL_SPACING * i) - Vector2.UnitY * ALPHABET_LENGTH * _font.LineSpacing)
                {
                    ColourHighlighted = _highlightColumn,
                    ColourNormal = _regularColour,
                };
                alphabit.ForEach(c => _letterMenuComponents[i].Add(c.ToString(), '0'));
                _letterMenuComponents[i].Add(" ", ' ');
                alphabit.ForEach(c => _letterMenuComponents[i].Add(c.ToString(), c));
                _letterMenuComponents[i].Add(" ", ' ');
                alphabit.ForEach(c => _letterMenuComponents[i].Add(c.ToString(), '0'));
                _letterMenuComponents[i].MenuIndex = ALPHABET_LENGTH;
                Components.Add(_letterMenuComponents[i]);
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var inputs = new[] { Game.PlayerOne, Game.PlayerTwo };
            string letterKeyPressed = inputs.First()
                .KeyStateNew.GetPressedKeys()
                .Where(k => inputs.First().KeyStateOld.IsKeyUp(k))
                .FirstOrDefault(k => Regex.IsMatch(k.ToString(), "^[A-Za-z]$")).ToString();
            if (letterKeyPressed.ToLower() != "none")
            {
                int maxLoops = ALPHABET_LENGTH + 1;
                while (letterKeyPressed != _letterMenuComponents[_activePosition].ActiveMenuItem.Name && maxLoops > 0)
                {
                    maxLoops--;
                    MoveRow(1);
                }
                MoveColumn(1);
            }
            else if (inputs.Any(p => p.ButtonPressed(Buttons.RightShoulder)))
                MoveRow(10);
            else if (inputs.Any(p => p.ButtonPressed(Buttons.LeftShoulder)))
                MoveRow(-10);
            else if (inputs.Any(p => p.ButtonPressed(Buttons.RightTrigger)))
                MoveRow(5);
            else if (inputs.Any(p => p.ButtonPressed(Buttons.LeftTrigger)))
                MoveRow(-5);
            else if (inputs.Any(p => p.KeyPressed(Keys.Space)))
                MoveColumn(1, false);
            else if (inputs.Any(p => p.ActionMenuLeft))
                MoveColumn(-1, false);
            else if (inputs.Any(p => p.ActionMenuRight))
                MoveColumn(1, false);
            else if (inputs.Any(p => p.ActionSelect))
                MoveColumn(1);
            else if (inputs.Any(p => p.ActionBack) || inputs.First().KeyPressed(Keys.Back))
                MoveColumn(-1);
            else if (inputs.Any(p => p.ActionMenuDown))
                MoveRow(1);
            else if (inputs.Any(p => p.ActionMenuUp))
                MoveRow(-1);

            base.Update(gameTime);
            if (_saveStatus == SaveStatus.StatusDrawn)
                SaveScore();
        }

        private void MoveColumn(int i, bool save = true)
        {
            Game.PlayMenuSound(1);
            _activePosition += i;
            var letters = _letterMenuComponents;
            if (save)
            {
                if (_activePosition < 0)
                {
                    Game.HighScore.Show();
                    Hide();
                }
                else if (_activePosition == letters.Length ||
                    (i > 0 && letters[_activePosition - i].ActiveMenuItem.Component == ' '))
                {
                    _saveStatus = SaveStatus.Requested;
                }
            }
            _activePosition = _activePosition.Mid(_letterMenuComponents.Length - 1);
            Recolour();
        }

        private void Recolour()
        {
            MenuComponent<char> active = _letterMenuComponents[_activePosition];
            active.ColourNormal = _highlightColumn;
            active.ColourHighlighted = _highlightChar;
            _letterMenuComponents.Where(m => m != active).ForEach(m => m.ColourNormal = _regularColour);
        }

        private void MoveRow(int i)
        {
            Game.PlayMenuSound(0);
            var active = _letterMenuComponents[_activePosition];
            active.MenuIndex = active.MenuIndex + i;
            if (active.MenuIndex < ALPHABET_LENGTH)
            {
                active.MenuIndex += ALPHABET_LENGTH +1;
                active.Position -= _font.LineSpacing * (ALPHABET_LENGTH + 1) * Vector2.UnitY;
            }
            else if (active.MenuIndex > ALPHABET_LENGTH*2)
            {
                active.MenuIndex -= ALPHABET_LENGTH + 1;
                active.Position += _font.LineSpacing * (ALPHABET_LENGTH + 1) * Vector2.UnitY;            
            }
            active.Position -= _font.LineSpacing*i*Vector2.UnitY;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            var cb = Game.Window.ClientBounds;
            var font = Game.DefaultGameFont;
            string[] title = { "Congratulations", string.Format("{0:n0} points is a new record!", Campaign.CurrentScore) };
            var bannerBox = new Rectangle(0, 0, cb.Width, cb.Height/3);
            Vector2 posVector2 = Vector2.UnitX*(cb.Width/2f);
            _spritebatch.Begin();
            _spritebatch.Draw(_background, bannerBox, bannerBox, Color.White, 0,
                Vector2.Zero, SpriteEffects.None, 1);
            bannerBox.Offset(0, cb.Height - bannerBox.Height);
            _spritebatch.Draw(_background, bannerBox, bannerBox, Color.White, 0,
                Vector2.Zero, SpriteEffects.None, 1);
            foreach (string s in title)
            {
                Vector2 adjustedVector2 = posVector2 - Vector2.UnitX*(font.MeasureString(s).X/2f);
                posVector2 += Vector2.UnitY * font.LineSpacing;
                _spritebatch.DrawString(font, s, adjustedVector2, _regularColour);                
            }

            if (_saveStatus == SaveStatus.Requested)
            {
                _spritebatch.Draw(SavingTexture, Vector2.Zero, Color.White);
                _saveStatus = SaveStatus.StatusDrawn;
            }

            _spritebatch.End();
        }

        private void SaveScore()
        {
            StringBuilder name = new StringBuilder(MAX_LETTERS);
            _letterMenuComponents.ForEach(l => name.Append(l.ActiveMenuItem.Component));

            
            _saveStatus = SaveStatus.Saving;
            Game.HighScore.UpdateHighScore(name.ToString().Trim(), Campaign.CurrentScore);
            _saveStatus = SaveStatus.Saved;

            Campaign.New(); // Reset the score
            Game.HighScore.Show();
            Hide();
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            base.OnEnabledChanged(sender, args);
            if (Enabled && _letterMenuComponents != null)
            {
                _activePosition = 0;
                Recolour();
            }
        }
    }
}
