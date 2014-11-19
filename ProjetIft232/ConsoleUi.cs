using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using ProjetIft232.Army;
using ProjetIft232.Buildings;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace ProjetIft232
{
    class ConsoleUi
    {
        private Game _Game;
        private Player player = new Player();
        public void Interact(Game game)
        {
            _Game = game;     
            // Game initial setup
            Console.WriteLine("Bienvenue!!!!");
            Console.WriteLine("Bonjour, quel est le nom de votre ville?");
            string cityName = Console.ReadLine();          
            player.Cities.Add(new City(cityName));
            player.NextCity();
            _Game.Players.Add(player);
            player.WriteXML();
            PrincipalMenu();
        }      

        private void PrincipalMenu()
        {
            int option = 0;
            while (option != 9)
            {
                Console.WriteLine("Tour #" + Game.TourIndex +@" 
0) Liste des villes de votre empire
1) Résumé de l'état de la ville
2) Création de bâtiments
3) Création d'armée
4) Créé Ville
5) Prochaine Ville
6) Prochain tour
7) Transfert de ressources
8) Save
9) Quitter
");
                option = int.Parse(Console.ReadLine());             
                switch (option)
                {
                    case 0:
                        Console.WriteLine("Vos villes : ");
                        player.Cities.ForEach(n => Console.WriteLine( string.Format("{0} \n", n.Name)));
                        break;
                    case 1:
                        ShowCurrentBuildings(_Game.CurrentPlayer.CurrentCity.Buildings);
                        if (_Game.CurrentPlayer.CurrentCity.Army.Count > 0)
                            ShowArmy(_Game.CurrentPlayer.CurrentCity.Army);
                        Console.WriteLine(_Game.CurrentPlayer.CurrentCity.Ressources);
                        break;
                    case 2:
                        BuildingMenu();
                        break;

                    case 3:
                        ArmyMenu();
                        break;
                    case 4:
                        CreateCity();
                        break;
                    case 5:
                        ChangeCityFocus();
                        break;
                    case 6:
                        Console.WriteLine(_Game.NextTurn());
                        Console.WriteLine("Nous avons progressé d'un tour, on ne va pas rester à l'âge de pierre");
                        break;
                    case 7:
                        TransferResources();
                        break;
                    case 8:
                        player.WriteXML();
                        break;
                    default:
                        break;
                }
            }
        }

        private void TransferResources()
        {
            string nomville;
            
            
            Console.WriteLine(" Entrez le nom de la ville bénéficiaire");
            nomville = Console.ReadLine();
            Console.WriteLine(" Quel type de ressources voulez-vous envoyer ");
            Console.WriteLine(@" 
1) Bois
2) Or
3) Viande
4) Pierre
5) Population
");
            bool result = false;
            int option = int.Parse(Console.ReadLine());  
            switch (option)
            {
                case 1:
                    Console.Write(" Combien de bois ? ");
                    int bois = Convert.ToInt32(Console.ReadLine());
                    result = _Game.Transfer(nomville, bois, 0, 0, 0, 0);
                    break;
                case 2:
                    Console.Write(" Combien d'or ? ");
                    int or = Convert.ToInt32(Console.ReadLine());
                    result = _Game.Transfer(nomville, 0, or, 0, 0, 0);
                    break;
                case 3:
                    Console.Write(" Combien de Viande ? ");
                    int viande = Convert.ToInt32(Console.ReadLine());
                    result = _Game.Transfer(nomville, 0, 0, viande, 0, 0);
                    break;
                case 4:
                      Console.Write(" Combien de Pierre ? ");
                    int pierre = Convert.ToInt32(Console.ReadLine());
                    result = _Game.Transfer(nomville, 0, 0, 0, pierre, 0);
                    break;
                case 5:
                      Console.Write(" Combien de Population ? ");
                    int pop = Convert.ToInt32(Console.ReadLine());
                    result = _Game.Transfer(nomville, 0, 0, 0, 0, pop);
                    break;

            }
            if (result)
            {
                Console.WriteLine("L'échange a bien eu lieu");
            }
            else
            {
                Console.WriteLine("L'échange n'a pas eu lieu");
            }



        }

        private void ChangeCityFocus()
        {
            _Game.ChangeCityFocus();
            Console.WriteLine(string.Format("Vous êtes maintenant dans la cité : {0}",player.CurrentCity.Name));
        }

        private void CreateCity()
        {
            Console.WriteLine("Quel est le nom de votre nouvelle ville?");
            string name = Console.ReadLine();
            if (_Game.CreateCity(name))
            {
                Console.WriteLine(string.Format("La ville {0} a été créée", name));
            }
            else
            {
                Console.WriteLine("Fond insuffisant");
            }
        }

        private void ArmyMenu()
        {
            var armyUnit = Enum.GetNames(typeof(ArmyUnitType));

            for (int i = 0; i < armyUnit.Length; i++)
            {
                Console.WriteLine("{0} : {1}", i + 1, armyUnit[i]);
            }
            int option = int.Parse(Console.ReadLine());


            if (_Game.CurrentPlayer.CurrentCity.AddArmy((ArmyUnitType)(option - 1)))
            {
                Console.WriteLine(" L'unité " + armyUnit[option - 1] + " a bien été créée");
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


            if (_Game.CurrentPlayer.CurrentCity.AddBuilding((BuildingType)(option - 1)))
            {
                Console.WriteLine(" Le bâtiment " + buildings[option - 1] + " a bien été créé");
            }
        }

        private void ShowCurrentBuildings(IEnumerable<Building> buildings)
        {
            foreach (var building in buildings)
            {
                Console.WriteLine(building.Name);
            }
        }

        private void ShowArmy(IEnumerable<ArmyUnit> army)
        {
            int[] units= new int[1];
            int Form = 0;
            foreach(var soldier in army)
            {
                if (!soldier.InFormation) {
                    if (soldier.Type == ArmyUnitType.Warrior)
                    {
                        units[0]++;
                    }}
            else {
                Form++;
                }
            }
            Console.WriteLine("Guerriers : "+units[0]);
            Console.WriteLine("Troupe en formation : " + Form);
        }

    }
}
