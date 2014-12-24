using Core.Technologies;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;

namespace Core.Buildings
{
    [DataContract]
    [KnownType(typeof (Market))]
    [KnownType(typeof (Casern))]
    public class Building : UpgradableEntity
    {
        public Building()
        {
        }

        public Building(int id, string name, string description, int turnsLeft, Resources resource,
            Requirement requirement)
            : base(id, name, description, turnsLeft, requirement)
        {
            Resource = resource;
        }


        public Building(Building build)
            : base(build)
        {
            Resource = build.Resource;
        }

        [DataMember]
        public Resources Resource { get; set; }
        public HashSet<int> CreatableUnit { get; private set; }


        //Retourne le nombre de ressources par batiment. Permet qu'un batiment actif (tel le marché) puisse générer de l'or 
        protected virtual Resources UpdateBuilding()
        {
            return Resource;
        }

        public Resources Update()
        {
            if (InConstruction)
            {
                return Build() 
                    ? new Resources(ResourcesType.Population, Requirement.Resources.Population) 
                    : Resources.Zero();
            }
            return UpdateBuilding();
        }

        public bool CanCreate(int type)
        {
            return CreatableUnit.Contains(type);
        }

        protected override void UpgradeEntity(Technology technology)
        {
            Resource += technology.Enhancements.Resources;
        }
    }
}