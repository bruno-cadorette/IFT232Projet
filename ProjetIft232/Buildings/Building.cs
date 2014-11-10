using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetIft232.Buildings
{
    public  class Building
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public bool InConstruction { get; protected set; }
        public BuildingType Type { get; protected set; }

        public Resource Resource { get; protected set; }

        public Requirement Requirement { get; protected set; }


        public int TurnsLeft { get; private set; }
        //Retourne le nombre de ressources par batiment. Permet qu'un batiment actif (tel le marché) puisse générer de l'or 
        protected virtual Resource UpdateBuilding()
        {
            return Resource;
        }


        public Building(BuildingType type, string name, string description, int turnsLeft, Resource resource, Requirement requirement)
        {
            Type = type;
            Name = name;
            Description = description;
            Resource = resource;
            InConstruction = false;
            TurnsLeft = turnsLeft;
            Requirement = requirement;
        }


        public Building(Building build)
        {
            Type = build.Type;
            Name = build.Name;
            Description = build.Description;
            Resource = build.Resource;
            InConstruction = true;
            TurnsLeft = build.TurnsLeft;
            Requirement = build.Requirement;
        }


        protected Building(int turnsLeft)
        {
            Resource = new Resource();
            InConstruction = true;
            TurnsLeft = turnsLeft;
        }

        public bool CanBeBuild(Resource actualResource, IEnumerable<Building> actualBuildings)
        {
            return Requirement.IsValid(actualResource, actualBuildings);
        }


        public Resource Update()
        {
            if (InConstruction)
            {
                Build();
                return Resource.Zero();
            }
            else
            {
                return UpdateBuilding();
            }
        }

        private void Build()
        {
            TurnsLeft--;
            InConstruction = TurnsLeft > 0;
        }

    }
}
