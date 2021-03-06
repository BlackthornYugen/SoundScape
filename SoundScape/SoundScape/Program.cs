using System.Linq;

namespace SoundScape
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameLoop game = new GameLoop(args.FirstOrDefault()))
            {
                game.Run();
            }
        }
    }
#endif
}

