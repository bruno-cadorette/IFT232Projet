using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetIft232.Buildings;

namespace ProjetIft232
{
    public class City
    {
        private List<Building> _Buildings;
        public int[] Ressources = new int[(int)Resources.End];
        public string Name { get; private set; }

        public City(string name)
        {
            Name = name;
            _Buildings = new List<Building>();
        }

        [UserCallable("test")]
        public CommandResult GetWorld()
        {
            return new CommandResult("Hello World");
        }

        public override string ToString()
        {
            return "Ville de "+Name;
        }

        //TODO: Changer quand l'UI va être bien
        public void AddBuilding(int building)
        {
            switch (building)
            {
                case 1 :
                    _Buildings.Add(new House());
                    break;
                case 2:
                    _Buildings.Add(new Farm());
                    break;
                case 3:
                    _Buildings.Add(new Mine());
                    break;
                case 4:
                    _Buildings.Add(new SawMill());
                    break;
                case 5:
                    _Buildings.Add(new Carry());
                    break;
                case 6:
                    _Buildings.Add(new Casern());
                    break;
                case 7:
                    _Buildings.Add(new School());
                    break;
                case 8:
                    _Buildings.Add(new Market());
                    break;
                case 9:
                    _Buildings.Add(new Hospital());
                    break;
            }
        }

        public void Update()
        {
            //Pour les ressources on ajoute a la variable tout ce qui est récupéré par chaque Update, 
            //cela nous permettra de savoir nos gains en ressource en debut de tour
            //Voici un exemple.
            int populationMultiplier = _Buildings.Sum(building => building.Update()[(int) Resources.Population]);

            UpdatePopulation(populationMultiplier);
        }

        private void UpdatePopulation(int mul)
        {
            Ressources[(int)Resources.Population] += mul * Sigmoid(Ressources[(int) Resources.Population]);
        }

        private static int Sigmoid(int t)
        {
            return Convert.ToInt32(1/(1 + Math.Exp(-t)));
        }

    }
}
