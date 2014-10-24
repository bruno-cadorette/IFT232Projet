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
        public string Name { get; private set; }

        public City(string name)
        {
            Name = name;
        }

        [UserCallable("test")]
        public CommandResult GetWorld()
        {
            return new CommandResult("Hello World");
        }

        public override string ToString()
        {
            return "Ville de "+Name;
        }
    }
}
