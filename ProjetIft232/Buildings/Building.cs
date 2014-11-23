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
    public class Building : BuildableEntity
    {
        
        [DataMember]
        public Resources Resource { get;  set; }

        [DataMember]
        private List<int> _CurrentTechnologies;

        
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
            _CurrentTechnologies = new List<int>();
        }


        public Building(Building build)
            : base(build)
        {
            Resource = build.Resource;
            _CurrentTechnologies = new List<int>();
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

        public void Upgrade(Technology technology)
        {
            _CurrentTechnologies.Add(technology.ID);
            Resource += technology.Enhancements.Resources;
        }

        public bool AlreadyApplied(int technology)
        {
           return _CurrentTechnologies.Any(n => n == technology);
        }

        public bool CanBeUpgraded(Resources actualResource, Technology technology)
        {
            return !InConstruction && technology.ApplicationCost <= actualResource && _CurrentTechnologies.All(x => x != technology.ID);
        }

        

        

    }
}
