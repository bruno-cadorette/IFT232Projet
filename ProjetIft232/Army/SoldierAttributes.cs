using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ProjetIft232.Army
{
    [DataContract]
    public class SoldierAttributes
    {
        [DataMember]
        public int Attack { get; set; }
        [DataMember]
        public int Defence { get; set; }

        public SoldierAttributes()
        {

        }
        public SoldierAttributes(int attack, int defence)
        {
            Attack = attack;
            Defence = defence;
        }

        public static SoldierAttributes operator +(SoldierAttributes a, SoldierAttributes b)
        {
            return new SoldierAttributes(a.Attack + b.Attack, a.Defence + b.Defence);
        }
    }
}
