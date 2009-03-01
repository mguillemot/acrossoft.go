using System;
using Acrossoft.Go;

namespace Acrossoft.Go
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (var game = new GoGame())
            {
                game.Run();
            }
        }
    }
}

