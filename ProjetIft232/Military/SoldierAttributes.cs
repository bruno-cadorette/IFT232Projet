using System.Runtime.Serialization;

namespace Core.Military
{
    [DataContract]
    public class SoldierAttributes
    {
        public SoldierAttributes()
        {
        }

        public SoldierAttributes(int attack, int defence, int health)
        {
            Attack = attack;
            Defence = defence;
            Health = health;
        }

        [DataMember]
        public int Attack { get; private set; }

        [DataMember]
        public int Defence { get; private set; }

        [DataMember]
        public int Health { get; private set; }

        public static SoldierAttributes operator +(SoldierAttributes a, SoldierAttributes b)
        {
            return new SoldierAttributes(a.Attack + b.Attack, a.Defence + b.Defence, a.Health + b.Health);
        }
        public static SoldierAttributes operator *(SoldierAttributes a, int n)
        {
            return new SoldierAttributes(a.Attack * n, a.Defence * n, a.Health * n);
        }
    }
}