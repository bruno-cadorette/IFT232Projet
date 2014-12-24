using System.Runtime.Serialization;
using Core.Technologies;

namespace Core.Army
{
    [DataContract]
    public class ArmyUnit : UpgradableEntity
    {
        public ArmyUnit(int id, string name, string desc, int attack, int def, int size, int turn, Resources transport,
            Requirement requirement)
            : base(id, name, desc, turn, requirement)
        {
            Attributes = new SoldierAttributes(attack, def);
            Transport = transport;
            Size = size;
            moral = 100;
        }

        public ArmyUnit(ArmyUnit army)
            : base(army)
        {
            Attributes = army.Attributes;
            Transport = army.Transport;
            Size = army.Size;
            moral = 100;
        }

        public SoldierAttributes Attributes { get; set; }


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

        public override string ToString()
        {
            return string.Format("{0} Atk : {1}, Def : {2}", Name, Attributes.Attack, Attributes.Defence);
        }

        protected override void UpgradeEntity(Technology technology)
        {
            Attributes += technology.Enhancements.SoldierAttributes;
        }
    }
}