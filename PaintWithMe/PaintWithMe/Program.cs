using System;

namespace PaintWithMe
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (PaintWithMeGame game = new PaintWithMeGame())
            {
                game.Run();
            }
        }
    }
#endif
}

