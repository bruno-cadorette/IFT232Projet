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
using ProjetIft232.Technologies;

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
            Console.WriteLine("Veuillez entrer le nombre de joueur pour la partie.");
            int numberOfPlayer = int.Parse(Console.ReadLine());

            for (int i = 1; i <= numberOfPlayer; i++)
            {
                Console.WriteLine("Quel est le nom du joueur {0}", i);
                string temp_playerName = Console.ReadLine();

                if (temp_playerName == "AI")
                {
                    Player player = new PlayerAI();
                    Console.WriteLine("Vous allez jouer contre {0} (Ville {1})", player.playerName, player.Cities.First().Name);
                    player.NextCity();
                    _Game.Players.Add(player);
                }
                else
                {
                    Player player = new Player();
                    player.playerName = temp_playerName;
                    Console.WriteLine("Bonjour, quel est le nom de votre ville?");
                    string cityName = Console.ReadLine();
                    player.Cities.Add(new City(cityName));
                    player.NextCity();
                    _Game.Players.Add(player);
                }
            }
            //player.WriteXML();

            Player[] regular = _Game.Players.Where(t => t.GetType() == typeof(Player)).ToArray();
            Player[] ais = _Game.Players.Where(t => t.GetType() == typeof(PlayerAI)).ToArray();
            _Game.Players.Clear();
            _Game.Players.AddRange(regular);
            _Game.Players.AddRange(ais);
            if (regular.Length > 0)
            {
                PrincipalMenu();
            }
            else
            {
                _Game.NextTurn();
            }
        }

        private void PrincipalMenu()
        {
            int option = 0;
            while (option != 12)
            {
                Console.WriteLine(@"Tour #{0}     Vous etes dans : {1} du joueur {2}
0) Liste des villes de votre empire
1) Résumé de l'état de la ville
2) Rechercher une Technologie
3) Création de bâtiments
4) Création d'armée
5) Créer Ville
6) Prochaine Ville
7) Envoi de ressources a une autre ville
8) Acheter des ressources : marché
9) Echanger des ressources
10) Appliquer une technologie
11) Prochain tour
12) Save
13) Quitter
", Game.TourIndex, _Game.CurrentPlayer.CurrentCity, _Game.CurrentPlayer.playerName);
                option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 0:
                        Console.WriteLine("Vos villes : ");
                        _Game.CurrentPlayer.Cities.ForEach(n => Console.WriteLine(string.Format("{0} \n", n.Name)));
                        break;
                    case 1:
                        ShowCurrentBuildings(_Game.CurrentPlayer.CurrentCity.Buildings);
                        if (_Game.CurrentPlayer.CurrentCity.Army.Count > 0)
                            ShowArmy(_Game.CurrentPlayer.CurrentCity.Army);
                        Console.WriteLine(_Game.CurrentPlayer.CurrentCity.Ressources);
                        break;

                    case 2:
                        TechMenu();
                        break;
                    case 3:
                        BuildingMenu();
                        break;

                    case 4:
                        ArmyMenu();
                        break;
                    case 5:
                        CreateCity();
                        break;
                    case 6:
                        ChangeCityFocus();
                        break;
                    case 7:
                        TransferResources();
                        break;
                    case 8:
                        AchatRessources();
                        break;
                    case 9:
                        // pas encore créé
                        ResourcesExchange();
                        break;
                    case 10:
                        ApplyTechMenu();
                        break;
                    case 11:
                        _Game.NextTurn();
                        Console.WriteLine("Nous avons progressé d'un tour, on ne va pas rester à l'âge de pierre");
                        break;
                    case 12:
                        _Game.CurrentPlayer.WriteXML();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ApplyTechMenu()
        {
            Console.WriteLine("Liste des technologies");
            var Techs = _Game.CurrentPlayer.ResearchedTech;
            int compteur = 0;
            Technology[] alltech = new Technology[Techs.Count];
            foreach (var technology in Techs)
            {
                Console.WriteLine(string.Format("{0}) {1}", compteur, technology.Name));
                alltech[compteur] = technology;
                compteur++;
            }
            Console.WriteLine("Choisissez une technologie à appliquer");
            int choix = int.Parse(Console.ReadLine());
            if (_Game.ApplyTech(alltech[choix].Name))
            {
                Console.WriteLine("La technologie a été appliquée au premier bâtiment.");
            }
            else
            {
                Console.WriteLine("La technologie n'est pas appliquable.");
            }
        }

        private void TechMenu()
        {
            Console.WriteLine("Liste des Technologies disponibles");
            var Techs = _Game.CurrentPlayer.GetTechnologies();
            int compteur = 0;
            Technology[] alltech = new Technology[Techs.Count];
            foreach (var technology in Techs)
            {
                Console.WriteLine(string.Format("{0}) {1}", compteur, technology.Key));
                alltech[compteur] = technology.Value;
                compteur++;
            }
            Console.WriteLine("Choisissez une technologie à rechercher");
            int choix = int.Parse(Console.ReadLine());
            if (_Game.ReshearchedTech(alltech[choix]))
            {
                Console.WriteLine("La technologie est en cours de recherche");
            }
            else
            {
                Console.WriteLine("Il vous manque des préalables pour rechercher cette technologie");
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


        private void PrintAchat(int accepte, ResourcesType type, int qtte, Market m)
        {
            switch (accepte)
            {
                case 1:
                    m.Achat(_Game.CurrentPlayer.CurrentCity, type, qtte);
                    break;
                case 2:
                    Console.WriteLine("Vous n'avez pas accepté la commande.");
                    break;
                default:
                    Console.WriteLine("Vous avez écrit n'importe quoi !! ");
                    return;
            }
        }


        private void AchatRessources()
        {

            if (_Game.getMarket() != null)
            {
                Market m = (Market)_Game.getMarket();

                Console.WriteLine(" Quel type de ressources voulez-vous acheter ");
                Console.WriteLine(@" 
1) Bois
2) Viande
3) Pierre
");
                bool result = false;
                int option = int.Parse(Console.ReadLine());
                int bois2, roche2, viande2;
                int accepte;
                switch (option)
                {
                    case 1:
                        Console.Write(" Combien de bois ? ");
                        int bois = Convert.ToInt32(Console.ReadLine());

                        bois2 = m.Conversion(bois, ResourcesType.Wood);
                        Console.Write(" Nous vous proposons " + bois2 + " unitées de bois contre " + bois2 / 15 + " unitées d'or : acceptez vous ?" + @"
1 : oui
2 : non");
                        PrintAchat(Convert.ToInt32(Console.ReadLine()), ResourcesType.Wood, bois2 / 15, m);
                        break;

                    case 2:
                        Console.Write(" Combien de viande ? ");
                        int viande = Convert.ToInt32(Console.ReadLine());

                        viande2 = m.Conversion(viande, ResourcesType.Meat);
                        Console.Write(" Nous vous proposons " + viande2 + " unitées de viande contre " + viande2 / 8 + " unitées d'or : acceptez vous ?" + @"
1 : oui
2 : non");
                        accepte = Convert.ToInt32(Console.ReadLine());
                        PrintAchat(Convert.ToInt32(Console.ReadLine()), ResourcesType.Meat, viande / 8, m);
                        break;
                    case 3:
                        Console.Write(" Combien de roche ? ");
                        int roche = Convert.ToInt32(Console.ReadLine());

                        roche2 = m.Conversion(roche, ResourcesType.Rock);
                        Console.Write(" Nous vous proposons " + roche2 + " unitées de roche contre " + roche2 / 12 + " unitées d'or : acceptez vous ?" + @"
1 : oui
2 : non");
                        accepte = Convert.ToInt32(Console.ReadLine());
                        PrintAchat(Convert.ToInt32(Console.ReadLine()), ResourcesType.Rock, roche2 / 12, m);
                        break;
                    default:
                        Console.WriteLine(" Message non interpreté");
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
            Console.WriteLine("Il n'y pas présentement pas de marché dans votre ville ou alors il est encore en construction");
        }

        private void ResourcesExchange()
        {
            if (_Game.getMarket() != null)
            {
                Market m = (Market)_Game.getMarket();

                Console.WriteLine(" Quel type de ressources donner à échanger ");
                Console.WriteLine(@" 
1) Bois
2) Viande
3) Pierre
");
                ResourcesType input = (ResourcesType)Enum.Parse(typeof(ResourcesType), Console.ReadLine());

                Console.WriteLine("Contre quoi?");
                Console.WriteLine(@" 
1) Bois
2) Viande
3) Pierre
");

                ResourcesType output = (ResourcesType)Enum.Parse(typeof(ResourcesType), Console.ReadLine());

                Console.WriteLine("Combien de {0} voulez vous echanger contre du {1}", input.ToString(), output.ToString());
                int qte = (int)Math.Min(_Game.CurrentPlayer.CurrentCity.Ressources[input], int.Parse(Console.ReadLine()));



                bool result = false;
                int option = int.Parse(Console.ReadLine());
                int bois2, roche2, viande2;
                int accepte;
                switch (option)
                {
                    case 1:
                        Console.WriteLine("Combien d'unités de bois souhaitez vous échanger ?");
                        Console.WriteLine("Combien d'unités de viande souhaitez vous échanger ?");
                        Console.WriteLine("Combien d'unités de  souhaitez vous échanger ?");
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }

            }
            Console.WriteLine("Il n'y pas présentement pas de marché dans votre ville ou alors il est encore en construction");
        }

        private void ChangeCityFocus()
        {
            _Game.ChangeCityFocus();
            Console.WriteLine(string.Format("Vous êtes maintenant dans la cité : {0}", _Game.CurrentPlayer.CurrentCity.Name));
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
            var armyUnit = ArmyLoader.GetInstance().Soldiers().ToArray();

            foreach (var army in armyUnit)
            {
                Console.WriteLine("{0} : {1}", army.ID + 1, army.Name);
            }
            int option = int.Parse(Console.ReadLine());


            if (_Game.CurrentPlayer.CurrentCity.AddArmy((option - 1)))
            {
                Console.WriteLine(" L'unité " + armyUnit[option - 1].Name + " a bien été créée");
            }
        }

        private void BuildingMenu()
        {
            var buildings = BuildingLoader.GetInstance().Buildings().ToArray();
            foreach (var building in buildings)
            {
                Console.WriteLine("{0} : {1}", building.ID + 1, building.Name);
            }
            int option = int.Parse(Console.ReadLine());


            if (_Game.CurrentPlayer.CurrentCity.AddBuilding(option - 1))
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
            foreach (var soldier in army.Where(x => !x.InConstruction).GroupBy(x => x.Name).Select(x => String.Format("{0} : {1}", x.Key, x.Count())))
            {
                Console.WriteLine(soldier);
            }
            Console.WriteLine("Troupe en formation : " + army.Count(x=>x.InConstruction));
        }

    }
}
