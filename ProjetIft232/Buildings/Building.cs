using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetIft232.Buildings
{
    public abstract class Building
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public bool InConstruction { get; protected set; }

        public Resource Ressource { get; protected set; }
        public int TurnsLeft { get; private set; }
        //Retourne le nombre de ressources par batiment. Permet qu'un batiment actif (tel le marché) puisse générer de l'or 
        //(SVP changer le int[] pour une meilleure structure (voir github))
        protected abstract Resource UpdateBuilding();


        protected Building(int turnsLeft)
        {
            Ressource = new Resource();
            InConstruction = true;
            TurnsLeft = turnsLeft;
        }

        public Resource Update()
        {
            if (InConstruction)
            {
                Build();
                return new Resource();
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
