using System;

namespace Mononoke
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Mononoke())
                game.Run();
        }
    }
}
