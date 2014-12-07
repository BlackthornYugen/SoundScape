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
        List<HighScoreSaved> lines = new List<HighScoreSaved>();

        public HighScore(GameLoop game, Texture2D texture, Texture2D background, Vector2 centerScreen, IEnumerable<HighScoreSaved> oldScores
            )
            : base(game, texture, background, centerScreen)
        {
            lines = bubbleSort(oldScores.ToList());
        }

        public bool isANewHighScore(int newScore)
        {
            if (lines.Count < 10)
            {
                return true;
            }
            else
            {
                List<int> tempScores = new List<int>();
                for (int i = 0; i < lines.Count; i++)
                {
                    tempScores.Add(lines[i].Score);
                }
                int min = ReturnMinimum(tempScores);
                if (newScore > min)
                {
                    return true;
                }
            }
            return false;
        }

        public void updateHighScore(string name, int score)
        {
            HighScoreSaved temp = new HighScoreSaved();
            temp.PlayerName = name;
            temp.Score = score;
            lines.Add(temp);
            lines = bubbleSort(lines);

            if (lines.Count >= 10)
            {
                lines.RemoveAt(lines.Count - 1);
            }
        }

        public List<HighScoreSaved> bubbleSort(List<HighScoreSaved> list)
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

        private int ReturnMinimum(List<int> list)
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
            SpriteFont regularFont = Game.Content.Load<SpriteFont>("fonts/regularFont");


            _spritebatch.Begin();
            
            _spritebatch.Draw(Texture, _centerScreen, Color.White);
            StringBuilder msg = new StringBuilder();
            lines.ForEach(line => msg.Append(string.Format("{0,-25} {1}\n", line.PlayerName, line.Score)));
            _spritebatch.DrawString(regularFont, msg, _centerScreen + new Vector2(60, 90), Color.Yellow, 0,
                Vector2.Zero, 1f, SpriteEffects.None, 0);
            _spritebatch.End();
        }
    }
}