using Core.Military;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Map
{
    public class Land
    {
        public int ID { get; set; }
        public SoldierAttributes AttackerBonus { get; set; }
        public SoldierAttributes DefenderBonus { get; set; }
        public bool CanBeTraveled { get; set; }
        public string Name { get; set; }
        public Land()
        {
            AttackerBonus = new SoldierAttributes();
            DefenderBonus = new SoldierAttributes();
            CanBeTraveled = true;
        }
    }
}
