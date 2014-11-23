using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Buildings;

namespace ProjetIft232.Technologies
{
    public class Technology
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Requirement Requirement { get; private set; }
        public int TurnsLeft { get; private set; }
        public bool InConstruction { get; private set; }
        public IEnumerable<int> AffectedBuilding { get; private set; }
        public Resources ApplicationCost { get; private set; }
        
        //Ca serait bien qu'une technologie puisse avoir differents effets sur differents batiments
        public Enhancement Enhancements { get; private set; }

        public Technology()
        {
            Requirement = Requirement.Zero();
            AffectedBuilding = Enumerable.Empty<int>();
            ApplicationCost = Resources.Zero();
            Enhancements = new Enhancement(new Resources(100, 100, 100, 100, 100), 0);
        }

        public Technology(Technology pattern)
        {
            Name = pattern.Name;
            Requirement = pattern.Requirement;
            AffectedBuilding = pattern.AffectedBuilding;
            ApplicationCost = pattern.ApplicationCost;
            Enhancements = pattern.Enhancements;
            TurnsLeft = pattern.TurnsLeft;
            InConstruction = true;
            ID = pattern.ID;
            Enhancements = pattern.Enhancements;
            Description = pattern.Description;
        }

        public Technology(int id, 
            string name, 
            string description, 
            Requirement requirement, 
            int turnsLeft, 
            IEnumerable<int> affectedBuilding,
            Resources applicationCost, 
            Enhancement enhancements)
        {
            ID = id;
            Name = name;
            Description = description;
            Requirement = requirement;
            TurnsLeft = turnsLeft;
            InConstruction = true;
            AffectedBuilding = affectedBuilding;
            ApplicationCost = applicationCost;
            Enhancements = enhancements;
        }


        public void Update()
        {
            if (InConstruction)
            {
                TurnsLeft--;
                InConstruction = TurnsLeft > 0;
            }

        }
    }
}
