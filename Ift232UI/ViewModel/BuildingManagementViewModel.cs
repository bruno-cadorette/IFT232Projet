using Core;
using Core.Buildings;
using GameHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ift232UI
{
    public class BuildingManagementViewModel
    {
        private City city;
        public IEnumerable<Building> BuildingTypes
        {
            get
            {
                return BuildingFactory.GetInstance().Buildings();
            }
        }
        public ICommand CreateBuilding { get; private set; }

        public BuildingManagementViewModel(City city)
        {
            this.city = city;
            ShowBuildingsStatus = new RelayCommand<Building>(x =>
            {
                BuildingCount = city.CountBuilding(x.ID, false);
                InConstructionCount = city.CountBuilding(x.ID, true);
            });
            CreateBuilding = new RelayCommand<Building>(x =>
            {
                city.AddBuilding(x.ID);
            },
            x=>x.CanBeBuild(city));
        }
        public int BuildingCount { get; set; }
        public int InConstructionCount { get; set; }
        public ICommand ShowBuildingsStatus { get; private set; }

    }
}
