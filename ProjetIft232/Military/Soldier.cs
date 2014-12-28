using System.Runtime.Serialization;
using Core.Technologies;

namespace Core.Military
{
    [DataContract]
    public class Soldier : UpgradableEntity
    {
        public Soldier(int id, string name, string desc, int attack, int defence, int health, int turn, Resources transport,
            Requirement requirement)
            : base(id, name, desc, turn, requirement)
        {
            Attributes = new SoldierAttributes(attack, defence, health);
            Transport = transport;
        }

        public Soldier(Soldier army)
            : base(army)
        {
            Attributes = army.Attributes;
            Transport = army.Transport;
        }

        public SoldierAttributes Attributes { get; set; }

        [DataMember]
        public Resources Transport { get; set; }


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