using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Army
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
        public Resources Transport
        {
            get
            {
                return Type.Transport * Size;
            }
        }

        public Groupment(Soldier soldier)
        {
            Type = soldier;
            Size = 1;

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
            Remove(damage / Type.Attributes.Health);
        }

        public override string ToString()
        {
            return Type.ToString() + " : " + Size;
        }
    }
}
