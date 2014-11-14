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

        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public bool InFormation { get; protected set; }

        public int Defense { get; protected set; }

        public ArmyUnitType Type { get; protected set; }

        public int TurnsLeft { get; protected set; }

        public int Attack { get; protected set; }
        public Requirement Requirement { get; protected set; }
        public Resources Transport { get; protected set; }


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
