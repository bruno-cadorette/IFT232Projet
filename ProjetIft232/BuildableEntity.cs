using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ProjetIft232.Buildings;
using ProjetIft232.Technologies;

namespace ProjetIft232
{
    [DataContract]
    [KnownType(typeof (Technology))]
    [KnownType(typeof (UpgradableEntity))]
    public abstract class BuildableEntity
    {
        public BuildableEntity()
        {
        }

        public BuildableEntity(BuildableEntity entity)
            : this(entity.ID, entity.Name, entity.Description, entity.TurnsLeft, entity.Requirement)
        {
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
        public int TurnsLeft { get; protected set; }


        protected bool Build()
        {
            TurnsLeft--;
            InConstruction = TurnsLeft > 0;
            return !InConstruction;
        }


        public bool CanBeBuild(Resources ressources, IEnumerable<Building> buildings, IEnumerable<Technology> techs)
        {
            return Requirement.IsValid(ressources, buildings, techs);
        }

        public bool CanBeBuild(Resources actualResource, IEnumerable<Building> actualBuildings)
        {
            return CanBeBuild(actualResource, actualBuildings.ToList(), new List<Technology>());
        }

        public void FinishConstruction()
        {
            TurnsLeft = 0;
            InConstruction = false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}