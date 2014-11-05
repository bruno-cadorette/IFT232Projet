using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using ProjetIft232.Buildings;
using System.Text;

namespace ProjetIft232
{
    class ConsoleUi
    {
        private Game _Game;
        public void Interact(Game game)
        {
            _Game = game;
            // Game initial setup
            Console.WriteLine("Bienvenue!!!!");
            Console.WriteLine("Bonjour, quel est le nom de votre ville?");
            string cityName = Console.ReadLine();
            var player = new Player();
            player.Cities.Add(new City(cityName));
            _Game.Players.Add(player);
            PrincipalMenu();

        }
        private void PrincipalMenu()
        {
            int option = 0;
            while (option != 4)
            {
                Console.WriteLine("Tour #" + Game.TourIndex +@" 
1) Résumé de l'état de la ville
2) Création de bâtiments
3) Prochain tour
4) Quitter
");
                option = int.Parse(Console.ReadLine());             
                switch (option)
                {
                    case 1:
                        ShowCurrentBuildings(_Game.CurrentPlayer.GetCity().Buildings);
                        Console.WriteLine(_Game.CurrentPlayer.GetCity().Ressources);
                        break;
                    case 2:
                        BuildingMenu();
                        break;

                    case 3:
                        _Game.NextTurn();
                        Console.WriteLine("Nous avons progressé d'un tour, on ne va pas rester à l'âge de pierre");
                        break;
                    default:
                        break;
                }
            }
        }

        private void BuildingMenu()
        {
            var buildings = Enum.GetNames(typeof(BuildingType));

            for (int i = 0; i < buildings.Length; i++)
            {
                Console.WriteLine("{0} : {1}", i + 1, buildings[i]);
            }
            int option = int.Parse(Console.ReadLine());


            if (_Game.CurrentPlayer.GetCity().AddBuilding((BuildingType)(option - 1)))
            {
                Console.WriteLine(" Le bâtiment" + buildings[option - 1] + "a bien été créé");
            }
        }

        private void ShowCurrentBuildings(IEnumerable<Building> buildings)
        {
            foreach (var building in buildings)
            {
                Console.WriteLine(building.Name);
            }
        }

    }
}
