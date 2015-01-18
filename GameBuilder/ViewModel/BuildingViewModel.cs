using Core.Buildings;
using GameHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameBuilder
{
    class BuildingViewModel : BindableBase
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetProperty(ref name, value);
            }
        }
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                SetProperty(ref description, value);
            }
        }
        private ResourceViewModel resources;
        public ResourceViewModel Resources
        {
            get
            {
                return resources;
            }
            set
            {
                SetProperty(ref resources, value);
            }
        }
        private ObservableCollection<BuildingViewModel> requirements;
        public ObservableCollection<BuildingViewModel> Requirements
        {
            get
            {
                return requirements;
            }
            set
            {
                SetProperty(ref requirements, value);
            }
        }
        public BuildingViewModel(Building building)
        {
            
            name = building.Name;
            description = building.Description;
            Resources = new ResourceViewModel(building.Resource);
            requirements = new ObservableCollection<BuildingViewModel>
                (building.Requirement.Buildings.Select(id => new BuildingViewModel(BuildingFactory.GetInstance().GetBuilding(id))));
        }
        public BuildingViewModel()
        {
            name = "New building";
            Resources = new ResourceViewModel();
            requirements = new ObservableCollection<BuildingViewModel>();
        }
        public override string ToString()
        {
            return Name;
        }
        public Building ToBuilding(int id)
        {
            return new Building();
        }
    }
}
