using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.Military;
using Core.Buildings;

namespace Core.Technologies
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
            AffectedBuildings = new HashSet<int>(affectedBuildings);
            AffectedSoldiers = new HashSet<int>(affectedSoldiers);
            ApplicationCost = applicationCost;
            Enhancements = enhancements;
        }

        [DataMember]
        public HashSet<int> AffectedBuildings { get; private set; }

        [DataMember]
        public HashSet<int> AffectedSoldiers { get; private set; }

        //Utiliser quand nous allons avoir des ID unique
        public HashSet<int> AffectedEntities { get; private set; }

        [DataMember]
        public Resources ApplicationCost { get; private set; }

        //Ca serait bien qu'une technologie puisse avoir differents effets sur differents batiments
        [DataMember]
        public Enhancement Enhancements { get; private set; }

        public bool CanAffect(UpgradableEntity entity)
        {
            if (entity is Building)
            {
                return AffectedBuildings.Contains(entity.ID);
            }
            else if (entity is Soldier)
            {
                return AffectedSoldiers.Contains(entity.ID);
            }
            else
            {
                return AffectedEntities.Contains(entity.ID);
            }
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