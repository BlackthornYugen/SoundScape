using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoundScape
{
    public struct HighScoreSaved
    {
        public string PlayerName;
        public int Score;
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class HighScoreScene : InfoScene
    {
        public const int HIGH_SCORE_LIMIT = 10;
        private List<HighScoreSaved> _highscores = new List<HighScoreSaved>();
        private List<HighScoreSaved> _highscoresOnline = new List<HighScoreSaved>();
        private bool _showOnline;

        public HighScoreScene(GameLoop game, Texture2D texture, Texture2D background, Vector2 centerScreen, IEnumerable<HighScoreSaved> oldScores
            )
            : base(game, texture, background, centerScreen)
        {
            _highscores = BubbleSort(oldScores.ToList());
        }

        public bool IsHighScore(int newScore)
        {
            var scoreLists = new[] {_highscores, _highscoresOnline};
            return scoreLists.Where(list => list != null).Any( list =>
            {
                // If the scoreboard isn't full
                if (_highscores.Count < HIGH_SCORE_LIMIT)
                    return true;

                // if newScore is higher than any of the current scores
                if (_highscores.Any(s => s.Score < newScore))
                    return true;

                // If we get here; newscore isn't a highscore on this list. 
                return false;
            });
        }

        public void UpdateHighScore(string name, int score)
        {
            var highScore = new HighScoreSaved();
            highScore.PlayerName = name;
            highScore.Score = score;
            _highscores.Add(highScore);
            _highscores = BubbleSort(_highscores);

            if (_highscores.Count >= HIGH_SCORE_LIMIT)
            {
                _highscores.RemoveAt(_highscores.Count - 1);
            }

            // Only bother saving to server if there is room.
            if (_highscoresOnline.Count >= HIGH_SCORE_LIMIT || _highscoresOnline.Any(s => s.Score < score)) 
                Toolbox.SaveObjectToFile(_highscores, "content/highscores.json");

            highScore.SaveScoreToDatabase();
        }

        private static List<HighScoreSaved> BubbleSort(List<HighScoreSaved> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].Score < list[i].Score)
                    {
                        HighScoreSaved temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
            }
            return list;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var inputs = new[] { Game.PlayerOne, Game.PlayerTwo };
            if (inputs.Any(i => i.ActionSelect) && Enabled)
            {
                // Flip online/offline scores
                _showOnline = !_showOnline;
                Game.PlayMenuSound(1);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteFont font = Game.DefaultGameFont;
            StringBuilder msg = new StringBuilder(new string(' ', 10));

            msg.Append(_showOnline ? "(Global Scores)\n" : "(Local Scores)\n");
            List<HighScoreSaved> drawScore = _showOnline ? _highscoresOnline : _highscores;

            _spritebatch.Begin();
            _spritebatch.Draw(Texture, _centerScreen, Color.White);
            drawScore.ForEach(line => msg.Append(string.Format("{0,-25} {1}\n", line.PlayerName, line.Score)));
            _spritebatch.DrawString(font, msg, _centerScreen - Vector2.UnitY * font.LineSpacing + new Vector2(60, 90), Color.Yellow, 0,
                Vector2.Zero, 1f, SpriteEffects.None, 0);
            _spritebatch.End();
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            var loadedScores = Toolbox.LoadScoresFromDatabase().ToList();
            if (Enabled)
            {
                _showOnline = !_showOnline;
                _highscoresOnline = loadedScores.Any() ? loadedScores : _highscoresOnline;
            }
            base.OnEnabledChanged(sender, args);
        }
    }
}