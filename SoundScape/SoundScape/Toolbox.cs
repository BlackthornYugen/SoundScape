using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using File = System.IO.File;

namespace SoundScape
{
    internal static class Toolbox
    {
        private const string SQL_CONN_STRING = @"SERVER=node.steelcomputers.com;DATABASE=soundscape;UID=soundscape;PASSWORD=zipzipzoom;";
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

        public static bool JsonSaveObject<T>(this T savedata, string filepath)
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                jsonSerializerSettings.Error = (sender, errArgs) => errArgs.ErrorContext.Handled = true;
                File.WriteAllText(filepath, JsonConvert.SerializeObject(savedata, Formatting.Indented, jsonSerializerSettings));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save {0}. Got message:\n{1}",
                    filepath, ex.GetBaseException().Message);
            }
            return false;
        }

        public static T JsonLoadObject<T>(string filepath) where T : new()
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(filepath), jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to load {0}. Got message:\n{1}",
                    filepath, ex.GetBaseException().Message);
            }
            return new T();
        }

        public static void JsonUpdateObject<T>(this T updateObject, string filepath)
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                JsonConvert.PopulateObject(File.ReadAllText(filepath), updateObject, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to update {0} using {1}. Got message:\n{2}",
                    updateObject, filepath, ex.GetBaseException().Message);
            }
        }

        public static void DatabaseSaveScore(this HighScoreSaved savedata)
        {
            var conn = new MySqlConnection(SQL_CONN_STRING);
            try
            {
                conn.Open();
                const string query = "INSERT INTO highscore (name, score) VALUES('{0}', '{1}')";
                var cmd = new MySqlCommand(string.Format(query, savedata.PlayerName, savedata.Score), conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save to database:\n{0}", ex.GetBaseException().Message);
            }
            finally
            {
                conn.Close();
            }
        }


        public static IEnumerable<HighScoreSaved> DatabaseLoadScores()
        {

            var conn = new MySqlConnection(SQL_CONN_STRING);
            MySqlDataReader reader = null;

            try
            {
                conn.Open();
                const string query = @"SELECT name, score FROM highscore ORDER BY score desc LIMIT {0};";
                var cmd = new MySqlCommand(string.Format(query, HighScoreScene.HIGH_SCORE_LIMIT), conn);
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
