using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetIft232.Buildings
{
    public  class Building
    {
        public string Name { get; set; }
        public string Description { get;  set; }
        public bool InConstruction { get;  set; }
        public BuildingType Type { get;  set; }

        public Resources Resource { get;  set; }

        public Requirement Requirement { get; set; }


        public int TurnsLeft { get; set; }
        //Retourne le nombre de ressources par batiment. Permet qu'un batiment actif (tel le marché) puisse générer de l'or 
        protected virtual Resources UpdateBuilding()
        {
            return Resource;
        }


        public Building(BuildingType type, string name, string description, int turnsLeft, Resources resource, Requirement requirement)
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

        public Building()
        {
            Type = BuildingType.Mine;
            Name = "toto";
            Description = "PUTQIN";
            Resource = new Resources(0,0,0,0,0);
            InConstruction = true;
            TurnsLeft = 3;
            Requirement = Requirement.Zero();
        }

        protected Building(int turnsLeft)
        {
            Resource = new Resources();
            InConstruction = true;
            TurnsLeft = turnsLeft;
        }

        public bool CanBeBuild(Resources actualResource, IEnumerable<Building> actualBuildings)
        {
            return Requirement.IsValid(actualResource, actualBuildings);
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

        private void Build()
        {
            TurnsLeft--;
            InConstruction = TurnsLeft > 0;
        }

    }
}
