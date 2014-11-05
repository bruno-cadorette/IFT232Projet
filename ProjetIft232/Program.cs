using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            ConsoleUi UI = new ConsoleUi();
            UI.Interact(game);
        }
    }
}
