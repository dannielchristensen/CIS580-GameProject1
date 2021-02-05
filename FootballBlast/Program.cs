using System;

namespace FootballBlast
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new FootballGame())
                game.Run();
        }
    }
}
