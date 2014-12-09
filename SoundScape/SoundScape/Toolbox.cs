using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using File = System.IO.File;

namespace SoundScape
{
    internal static class Toolbox
    {
        private const string sqlConnString = @"SERVER=node.steelcomputers.com;DATABASE=soundscape;UID=soundscape;PASSWORD=zipzipzoom;";
        public static void ForEach<T>(this IEnumerable<T> value, Action<T> action)
        {
            foreach (T item in value)
            {
                action(item);
            }
        }

        public static int Mid(this int value, int max, int min = 0)
        {
            return Math.Min(max, Math.Max(min, value));
        }

        public static bool SaveObjectToFile<T>(T savedata, string filepath)
        {
            try
            {
                File.WriteAllText(filepath, JsonConvert.SerializeObject(savedata, Formatting.Indented));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save {0}. Got message:\n{1}",
                    filepath, ex.GetBaseException().Message);
            }
            return false;
        }

        public static T LoadObjectFromFile<T>(string filepath) where T : new()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(filepath));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to load {0}. Got message:\n{1}", 
                    filepath, ex.GetBaseException().Message);
            }
            return new T();
        }

        public static bool SaveScoreToDatabase(this HighScoreSaved savedata)
        {
            var conn = new MySqlConnection(sqlConnString);
            try
            {
                conn.Open();
                const string query = "INSERT INTO highscore (name, score) VALUES('{0}', '{1}')";
                var cmd = new MySqlCommand(string.Format(query, savedata.PlayerName, savedata.Score), conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save to database:\n{0}", ex.GetBaseException().Message);
            }
            finally
            {
                conn.Close();
            }
            return false;
        }


        public static IEnumerable<HighScoreSaved> LoadScoresFromDatabase()
        {

            var conn = new MySqlConnection(sqlConnString);
            MySqlDataReader reader = null;

            try
            {
                conn.Open();
                const string query = @"SELECT name, score FROM highscore ORDER BY score desc LIMIT {0};";
                var cmd = new MySqlCommand(string.Format(query, HighScore.HIGH_SCORE_LIMIT), conn);
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to execute query to database:\n{0}", ex.GetBaseException().Message);
            } 

            while (reader != null && reader.Read())
            {
                int score;
                if (int.TryParse(reader["score"].ToString(), out score))
                    yield return new HighScoreSaved {PlayerName = reader["name"].ToString(), Score = score};
            }

            try
            {
                if (reader != null) reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to close reader:\n{0}", ex.GetBaseException().Message);
            } 

            try
            {
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to close connection to database:\n{0}", ex.GetBaseException().Message);
            } 
        } 
    }
}
