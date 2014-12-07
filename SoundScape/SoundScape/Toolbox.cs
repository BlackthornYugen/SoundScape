using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using File = System.IO.File;

namespace SoundScape
{
    internal static class Toolbox
    {
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
    }
}
