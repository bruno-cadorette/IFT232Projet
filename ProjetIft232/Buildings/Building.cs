using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetIft232.Technologies;
using System.Runtime.Serialization;

namespace ProjetIft232.Buildings
{
    [DataContract]
    [KnownType(typeof(Market))]
    public class Building : UpgradableEntity
    {
        
        [DataMember]
        public Resources Resource { get;  set; }

        

        
        //Retourne le nombre de ressources par batiment. Permet qu'un batiment actif (tel le marché) puisse générer de l'or 
        protected virtual Resources UpdateBuilding()
        {
            return Resource;
        }


        public Building()
        {

        }
        public Building(int id, string name, string description, int turnsLeft, Resources resource, Requirement requirement)
            : base(id, name, description, turnsLeft, requirement)
        {
            Resource = resource;
        }


        public Building(Building build)
            : base(build)
        {
            Resource = build.Resource;
        }




        public Resources Update()
        {
            if (InConstruction)
            {
                Build();
                return Resources.Zero();
            }
            else
            {
                return UpdateBuilding();
            }
        }

        protected override void UpgradeEntity(Technology technology)
        {
            Resource += technology.Enhancements.Resources;
        }
    }
}
