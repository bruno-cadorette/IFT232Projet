using Core;
using Core.Buildings;
using Core.Military;
using Core.Technologies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ift232UI
{
    public class CityManagementViewModel
    {
        private City city;

        public BuildingManagementViewModel BuildingManagementViewModel { get; set; }

        public IEnumerable<Soldier> UnitTypes
        {
            get
            {
                return ArmyFactory.GetInstance().Soldiers();
            }
        }
        public Soldier SelectedSoldier { get; set; }


        

        public bool CanRecruitSoldiers
        {
            get
            {
                return city.FinishedBuildings.Any(t => t is Casern); 
            }
        }

        public bool CanTrade
        {
            get
            {
                return city.FinishedBuildings.Any(t => t is Market); 
            }
        }

        public CityManagementViewModel(City city)
        {
            this.city = city;
            BuildingManagementViewModel = new BuildingManagementViewModel(city);
        }

   
    }
}
