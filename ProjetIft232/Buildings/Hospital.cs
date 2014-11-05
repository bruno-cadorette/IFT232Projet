using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    public class Hospital : Building
    {
        public Hospital()
            : base(3)
        {
            Name = "Hôpital";
            Description =
                "Un bâtiment permettant d'accepter et de soigner les malades.\nQui sait, peut-être qu'un jour il permettra d'endiguer l'Ebola...";
        }
    }
}
