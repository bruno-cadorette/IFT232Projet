using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetIft232
{
    class Player
    {
        public List<City> Cities { get; set; }

        public Player()
        {
            Cities = new List<City>();
           
        }

        public City GetCity()
        {
            return Cities.First();
        }


        public override string ToString()
        {
            return "Joueur 1";
        }
    }
}
