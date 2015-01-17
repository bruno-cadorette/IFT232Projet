using Core;
using Core.Buildings;
using Core.Military;
using Core.Technologies;
using GameHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ift232UI
{
    public class CityManagementViewModel : BindableBase
    {
        private City city;

        public BuildingManagementViewModel BuildingManagementViewModel { get; set; }
        public ArmyManagementViewModel ArmyManagementViewModel { get; set; }


        
        public string CityName
        {
            get
            {
                return city.Name;
            }
        }
        public Resources Resources
        {
            get
            {
                return city.Ressources;
            }
        }
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
            ArmyManagementViewModel = new ArmyManagementViewModel(city);
            BuildingManagementViewModel.PropertyChanged += ChangeResouresEvent;
            ArmyManagementViewModel.PropertyChanged += ChangeResouresEvent;
        }
        private void ChangeResouresEvent(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Resources")
            {
                OnPropertyChanged("Resources");
            }

        }
    }
}
