using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Buildings
{
    [DataContract]
    public class Casern : Building
    {
        public Casern(Building building)
            : base(building)
        {
        }
    }
}