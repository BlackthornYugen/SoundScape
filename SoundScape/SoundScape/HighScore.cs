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
    public class HighScore : InfoScene
    {
        List<HighScoreSaved> _highscores = new List<HighScoreSaved>();

        public HighScore(GameLoop game, Texture2D texture, Texture2D background, Vector2 centerScreen, IEnumerable<HighScoreSaved> oldScores
            )
            : base(game, texture, background, centerScreen)
        {
            _highscores = BubbleSort(oldScores.ToList());
        }

        public bool IsHighScore(int newScore)
        {
            if (_highscores.Count < 10)
            {
                return true;
            }
            else
            {
                List<int> tempScores = new List<int>();
                for (int i = 0; i < _highscores.Count; i++)
                {
                    tempScores.Add(_highscores[i].Score);
                }
                int min = ReturnMinimum(tempScores);
                if (newScore > min)
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateHighScore(string name, int score)
        {
            var highScore = new HighScoreSaved();
            highScore.PlayerName = name;
            highScore.Score = score;
            _highscores.Add(highScore);
            _highscores = BubbleSort(_highscores);

            if (_highscores.Count >= 10)
            {
                _highscores.RemoveAt(_highscores.Count - 1);
            }

            Toolbox.SaveObjectToFile(_highscores, "content/highscores.json");
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

        private static int ReturnMinimum(List<int> list)
        {
            int min = list[0];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] < min)
                {
                    min = list[i];
                }
            }
            return min;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            var regularFont = Game.DefaultGameFont;
            var msg = new StringBuilder();


            _spritebatch.Begin();
            
            _spritebatch.Draw(Texture, _centerScreen, Color.White);
            _highscores.ForEach(line => msg.Append(string.Format("{0,-25} {1}\n", line.PlayerName, line.Score)));
            _spritebatch.DrawString(regularFont, msg, _centerScreen + new Vector2(60, 90), Color.Yellow, 0,
                Vector2.Zero, 1f, SpriteEffects.None, 0);
            _spritebatch.End();
        }
    }
}