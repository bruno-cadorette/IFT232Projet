using ProjetIft232.Army;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetIft232.Technologies
{
    [DataContract]
    public sealed class Enhancement
    {
        [DataMember]
        public Resources Resources { get; private set; }
        [DataMember]
        public int ConstructionTime { get; private set; }

        public SoldierAttributes SoldierAttributes { get; private set; }


        public Enhancement(Resources resources, SoldierAttributes soldierAttributes, int constructionTime)
        {
            Resources = resources;
            SoldierAttributes = soldierAttributes;
            ConstructionTime = constructionTime;
        }

        public static Enhancement Zero()
        {
            return new Enhancement(Resources.Zero(), new SoldierAttributes(), 0);
        }
    }
}
