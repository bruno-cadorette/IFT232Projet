using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetIft232.Buildings;
using System.Runtime.Serialization;

namespace ProjetIft232.Technologies
{
    [DataContract]
    public class Technology : BuildableEntity
    {
        [DataMember]
        public IEnumerable<int> AffectedBuilding { get; private set; }
        [DataMember]
        public Resources ApplicationCost { get; private set; }
        
        //Ca serait bien qu'une technologie puisse avoir differents effets sur differents batiments
        [DataMember]
        public Enhancement Enhancements { get; private set; }

        public Technology()
        {
        }

        public Technology(Technology technology)
            : base(technology)
        {
            AffectedBuilding = technology.AffectedBuilding;
            ApplicationCost = technology.ApplicationCost;
            Enhancements = technology.Enhancements;
        }

        public Technology(int id, 
            string name, 
            string description, 
            Requirement requirement, 
            int turnsLeft, 
            IEnumerable<int> affectedBuilding,
            Resources applicationCost, 
            Enhancement enhancements)
            :base(id,name,description,turnsLeft,requirement)
        {
            AffectedBuilding = affectedBuilding;
            ApplicationCost = applicationCost;
            Enhancements = enhancements;
        }


        public void Update()
        {
            if (InConstruction)
            {
                Build();
            }
        }
    }
}
