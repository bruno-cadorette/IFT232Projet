using ProjetIft232.Buildings;
using ProjetIft232.Technologies;
using ProjetIft232.Army;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ProjetIft232
{
    [DataContract]
    [KnownType(typeof(Building))]
    [KnownType(typeof(ArmyUnit))]
    public abstract class BuildableEntity
    {
        [DataMember]
        public int ID { get; private set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public string Description { get; private set; }
        [DataMember]
        public bool InConstruction { get; private set; }
        [DataMember]
        public Requirement Requirement { get; private set; }
        [DataMember]
        public int TurnsLeft { get; private set; }
        public BuildableEntity()
        {

        }

        public BuildableEntity(BuildableEntity entity)
        {
            ID = entity.ID;
            Name = entity.Name;
            Description = entity.Description;
            TurnsLeft = entity.TurnsLeft;
            Requirement = entity.Requirement;
            InConstruction = true;
        }

        public BuildableEntity(int id, string name, string description, int turnsLeft, Requirement requirement)
        {
            ID = id;
            Name = name;
            Description = description;
            TurnsLeft = turnsLeft;
            Requirement = requirement;
            InConstruction = true;
        }

        protected void Build()
        {
            TurnsLeft--;
            InConstruction = TurnsLeft > 0;
        }

        public void ReduceTurnLeft(int minus)
        {
            TurnsLeft = ((TurnsLeft - minus) < 0) ? 0 : TurnsLeft - minus;
        }
        public bool CanBeBuild(Resources ressources, List<Building> buildings, List<Technology> techs)
        {
            return Requirement.IsValid(ressources, buildings, techs);
        }

        public bool CanBeBuild(Resources actualResource, IEnumerable<Building> actualBuildings)
        {
            return CanBeBuild(actualResource, actualBuildings.ToList<Building>(), new List<Technology>());
        }

        public void FinishConstruction()
        {
            TurnsLeft = 0;
            InConstruction = false;
        }



    }
}
