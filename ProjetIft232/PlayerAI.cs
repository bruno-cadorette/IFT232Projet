using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using ProjetIft232.Technologies;
using ProjetIft232.Buildings;
using ProjetIft232.Army;

namespace ProjetIft232
{
    public class PlayerAI : Player
    {
        Random rd = new Random();
        string[] CityNames_Prefix = { "Le grand ", "St-", "Saint-", "Prais ", "Notre-Dame-De-", "Patate-" };
        string[] CityNames_Suffix = { "Jeanne d'arc", "Cristo", "Fred", "Bruno", "Jean-Francois 3e", "Mélanie", "Anfray", "Dimitri", "Samuel" };
        string[] playerNames = { "Jackson", "Aiden", "Liam", "Lucas", "Noah", "Mason", "Jayden", "Ethan", "Jacob", "Jack", "Caden" };
        public PlayerAI()
        {
            Cities.Add(new City(CityNames_Prefix[rd.Next(CityNames_Prefix.Length)] + CityNames_Suffix[rd.Next(CityNames_Suffix.Length)]));
            playerName = playerNames[rd.Next(playerNames.Length)];
        }

        public void Play()
        {
            int actions = rd.Next(10);
            var buildingValues = BuildingLoader.GetInstance().Buildings().Select(x => x.ID).ToArray();
            var armyValues = ArmyLoader.GetInstance().Soldiers().Select(x => x.ID).ToArray();
            for (int i = 0; i < actions; i++)
            {
                int actionTaken = rd.Next(10);
                City cityTaken = Cities[rd.Next(Cities.Count)];
                switch (actionTaken)
                {
                    case 0: // Random army creation
                        {
                            cityTaken.AddArmy(armyValues[rd.Next(armyValues.Length)]);
                        }
                        break;
                    case 2:
                    case 1: // Random building making
                        {

    
                            
                            cityTaken.AddBuilding(buildingValues[rd.Next(buildingValues.Length)]);
                        }
                        break;
                    case 3: // destruction aleatoire (why? no idea.)
                        if(cityTaken.Buildings.Any())
                            cityTaken.RemoveBuilding(rd.Next(cityTaken.Buildings.Count));
                        break;
                    default:
                        // Intentionally empty
                        break;
                }

            }
        }
    }
}
