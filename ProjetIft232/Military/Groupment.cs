using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Military
{
    [DataContract]
    public class Groupment
    {
        [DataMember]
        public Soldier Type { get; private set; }
        public SoldierAttributes Attributes
        {
            get
            {
                return Type.Attributes * Size;
            }
        }
        [DataMember]
        public int Size { get; private set; }
        private int health;
        public Resources Transport
        {
            get
            {
                return Type.Transport * Size;
            }
        }

        public Groupment(Soldier soldier):this(soldier, 1)
        {
        }
        public Groupment(Soldier soldier, int size)
        {
            Type = soldier;
            Size = size;
            health = Type.Attributes.Health;
        }
        public void Add(int n)
        {
            Size += n;
        }
        public void Remove(int n)
        {
            Size = Math.Max(Size - n, 0);
        }
        public void TakeDamage(int damage)
        {
            var currentDamage = health - damage;
            if (currentDamage < 0)
            {
                Remove(1 + (currentDamage * -1 / Type.Attributes.Health));
            }
            else
            {
                health = currentDamage;
            }
        }

        public override string ToString()
        {
            return Type.ToString() + " : " + Size;
        }
    }
}
