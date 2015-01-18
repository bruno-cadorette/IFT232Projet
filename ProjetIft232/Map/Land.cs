using Core.Military;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Map
{
    [DataContract]
    public class Land
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public SoldierAttributes AttackerBonus { get; set; }
        [DataMember]
        public SoldierAttributes DefenderBonus { get; set; }
        [DataMember]
        public bool CanBeTraveled { get; set; }
        [DataMember]
        public string Name { get; set; }
        public Land()
        {
            AttackerBonus = new SoldierAttributes();
            DefenderBonus = new SoldierAttributes();
            CanBeTraveled = true;
        }
    }
}
