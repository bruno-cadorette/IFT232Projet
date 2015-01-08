using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.Buildings;
using Core.Technologies;

namespace Core
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

        public bool CanBeBuild(City city)
        {
            return CanBeBuild(city.Ressources, city.FinishedBuildings, city.ResearchedTechnologies);
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
        public override bool Equals(object obj)
        {
            var b = obj as BuildableEntity;
            if ((object)b == null)
            {
                return false;
            }
            else
            {
                return ID == b.ID && InConstruction == b.InConstruction && TurnsLeft == b.TurnsLeft;
            }
        }
        public static bool operator ==(BuildableEntity a, BuildableEntity b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }

            return a.Equals(b);
        }
        public static bool operator !=(BuildableEntity a, BuildableEntity b)
        {
            return !(a == b);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}