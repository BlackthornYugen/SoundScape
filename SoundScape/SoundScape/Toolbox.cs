using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
