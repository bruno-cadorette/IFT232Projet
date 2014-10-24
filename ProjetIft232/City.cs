using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetIft232
{
    class City
    {
        public List<Building> Buildings { get; set; }
        public int[] Ressources = new int[(int)ResourceType.End];

        [UserCallable("test")]
        public CommandResult GetWorld()
        {
            return new CommandResult("Hello World");
        }

        
    }
}
