using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Buildings;

namespace ProjetIft232.Army
{
    public class ArmyUnit
    {
        protected ArmyUnit(string nom, string desc, int attack, int def, ArmyUnitType armyUnitType, int turn)
        {
            Name = nom;
            Description = desc;
            Attack = attack;
            Defense = def;
            Type = armyUnitType;
            TurnsLeft = turn;
            InFormation = true;

        }

        protected ArmyUnit()
        {
            Name = "Chevre";
            Description = "toto";
            Attack = 5;
            Defense = 5;
            Type = ArmyUnitType.Warrior;
            TurnsLeft = 0;
            InFormation = true;

        }


        public string Name { get; set; }
        public string Description { get; set; }
        public bool InFormation { get; set; }

        public int Defense { get; set; }

        public ArmyUnitType Type { get; set; }

        public int TurnsLeft { get; set; }

        public int Attack { get; set; }
        public Requirement Requirement { get; set; }
        public Resources Transport { get; set; }


        public void Update()
        {
            if (InFormation)
            {
                Build();
            }
        }

        private void Build()
        {
            TurnsLeft--;
            InFormation = TurnsLeft > 0;
        }

        public bool CanBeBuild(Resources ressources, List<Building> buildings)
        {
            return Requirement.IsValid(ressources, buildings);
        }
    }
}
