using System.Runtime.Serialization;

namespace ProjetIft232.Army
{
    [DataContract]
    public class SoldierAttributes
    {
        public SoldierAttributes()
        {
        }

        public SoldierAttributes(int attack, int defence)
        {
            Attack = attack;
            Defence = defence;
        }

        [DataMember]
        public int Attack { get; set; }

        [DataMember]
        public int Defence { get; set; }

        public static SoldierAttributes operator +(SoldierAttributes a, SoldierAttributes b)
        {
            return new SoldierAttributes(a.Attack + b.Attack, a.Defence + b.Defence);
        }
    }
}