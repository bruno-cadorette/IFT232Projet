using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ProjetIft232.Army;
using ProjetIft232.Buildings;

namespace ProjetIft232.Technologies
{
    [DataContract]
    public class Technology : BuildableEntity
    {
        public Technology()
        {
        }

        public Technology(Technology technology)
            : base(technology)
        {
            AffectedBuildings = technology.AffectedBuildings;
            AffectedSoldiers = technology.AffectedSoldiers;
            ApplicationCost = technology.ApplicationCost;
            Enhancements = technology.Enhancements;
        }

        public Technology(int id,
            string name,
            string description,
            Requirement requirement,
            int turnsLeft,
            IEnumerable<int> affectedBuildings,
            IEnumerable<int> affectedSoldiers,
            Resources applicationCost,
            Enhancement enhancements)
            : base(id, name, description, turnsLeft, requirement)
        {
            AffectedBuildings = affectedBuildings;
            AffectedSoldiers = affectedSoldiers;
            ApplicationCost = applicationCost;
            Enhancements = enhancements;
        }

        [DataMember]
        public IEnumerable<int> AffectedBuildings { get; private set; }

        [DataMember]
        public IEnumerable<int> AffectedSoldiers { get; private set; }

        [DataMember]
        public Resources ApplicationCost { get; private set; }

        //Ca serait bien qu'une technologie puisse avoir differents effets sur differents batiments
        [DataMember]
        public Enhancement Enhancements { get; private set; }

        public bool CanAffect(UpgradableEntity entity)
        {
            if (entity is Building)
            {
                return AffectedBuildings.Any(x => x == entity.ID);
            }
            if (entity is ArmyUnit)
            {
                return AffectedSoldiers.Any(x => x == entity.ID);
            }
            throw new NotImplementedException("Vous n'avez pas prévu de cas pour cette upgradable entity!");
        }

        public bool CanAffect(ArmyUnit soldier)
        {
            return AffectedSoldiers.Any(x => x == soldier.ID);
        }

        public void Update()
        {
            if (InConstruction)
            {
                Build();
            }
        }
    }
}