using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class Casern : Building
    {
        public Casern()
            : base(3)
        {
            Name = "Casern";
            Description ="Un bâtiment permettant l'entrainement de soldats";
        }
    }
}