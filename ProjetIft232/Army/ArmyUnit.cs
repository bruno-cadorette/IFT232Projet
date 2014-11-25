using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Buildings;
using ProjetIft232.Technologies;
using System.Runtime.Serialization;

namespace ProjetIft232.Army
{
    [DataContract]
    public class ArmyUnit : BuildableEntity
    {
        public ArmyUnit(int id, string name, string desc, int attack, int def, int size, int turn,Resources transport, Requirement requirement)
            : base(id, name, desc, turn, requirement)
        {
            Attack = attack;
            Defense = def;
            Transport = transport;
            Size = size;
            moral = 100;
        }

        public ArmyUnit(ArmyUnit army)
            :base(army)
        {
            Attack = army.Attack;
            Defense = army.Defense;
            Transport = army.Transport;
            Size = army.Size;
            moral = 100;
        }


        [DataMember]
        public int Attack { get; set; }
        [DataMember]
        public int Defense { get; set; }

        [DataMember]
        public int Size { get; set; }


       [DataMember]
        public Resources Transport { get; set; }

       public int moral { get; set; }


        public void Update()
        {
            if (InConstruction)
            {
                Build();
            }
        }


    }
}
