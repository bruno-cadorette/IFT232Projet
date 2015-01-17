using System;
using System.Linq;
using Core.Military;
using Core.Buildings;

namespace Core
{
    public class PlayerAI : Player
    {
        private readonly string[] CityNames_Prefix =
        {
            "Le grand ", "St-", "Saint-", "Prais ", "Notre-Dame-De-",
            "Patate-"
        };

        private readonly string[] CityNames_Suffix =
        {
            "Jeanne d'arc", "Cristo", "Fred", "Bruno", "Jean-Francois 3e",
            "Mélanie", "Anfray", "Dimitri", "Samuel"
        };

        private readonly string[] playerNames =
        {
            "Jackson", "Aiden", "Liam", "Lucas", "Noah", "Mason", "Jayden", "Ethan",
            "Jacob", "Jack", "Caden"
        };

        private readonly Random rd = new Random();

        public PlayerAI()
        {
            Cities.Add(
                new City(CityNames_Prefix[rd.Next(CityNames_Prefix.Length)] +
                         CityNames_Suffix[rd.Next(CityNames_Suffix.Length)]));
            playerName = playerNames[rd.Next(playerNames.Length)];
        }

        public void Play()
        {
            int actions = rd.Next(10);
            var buildingValues = BuildingFactory.GetInstance().Buildings().Select(x => x.ID).ToArray();
            var armyValues = ArmyFactory.GetInstance().Soldiers().Select(x => x.ID).ToArray();
            for (int i = 0; i < actions; i++)
            {
                int actionTaken = rd.Next(10);
                City cityTaken = Cities[rd.Next(Cities.Count)];
                switch (actionTaken)
                {
                    case 0: // Random army creation
                    {
                        cityTaken.AddArmy(armyValues[rd.Next(armyValues.Length)], 1);
                    }
                        break;
                    case 2:
                    case 1: // Random building making
                    {
                        cityTaken.AddBuilding(buildingValues[rd.Next(buildingValues.Length)]);
                    }
                        break;
                    case 3: // destruction aleatoire (why? no idea.)
                        if (cityTaken.Buildings.Any())
                            cityTaken.RemoveBuilding(rd.Next(cityTaken.Buildings.Count));
                        break;
// ReSharper disable once RedundantEmptyDefaultSwitchBranch
                    default:
                        // Intentionally empty
                        break;
                }
            }
        }
    }
}