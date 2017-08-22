using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoMansEngine
{
    class Program
    {
        static void Main()
        {
            GAME game = new GAME();
            game.start();
            while(true)
            {
                game.key = Console.ReadKey(true).Key.ToString();
            }
        }
    }
}
