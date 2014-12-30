using Core.Buildings;
using GameBuilder.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GameBuilder
{
    class EntityCreationViewModel : BindableBase
    {
        private BuildingViewModel currentBuilding;
        public BuildingViewModel CurrentBuilding
        {
            get
            {
                return currentBuilding;
            }
            set
            {
                
                SetProperty<BuildingViewModel>(ref currentBuilding, value);
            }
        }
        public ObservableCollection<BuildingViewModel> Buildings { get; set; }
        public string Test { get; set; }
        public ICommand NewCommand { get; private set; }
        public ICommand AddToRequirementCommand { get; private set; }
        public ICommand SelectCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SerializeComand { get; private set; }
        public EntityCreationViewModel()
        { 
            Buildings = new ObservableCollection<BuildingViewModel>(BuildingFactory.GetInstance().Buildings().Select(x => new BuildingViewModel(x)));
            currentBuilding = new BuildingViewModel();
            NewCommand = new RelayCommand<BuildingViewModel>(x =>
                {
                    CurrentBuilding = new BuildingViewModel();
                    Buildings.Add(CurrentBuilding);
                });
            SelectCommand = new RelayCommand<BuildingViewModel>(x => CurrentBuilding = x);
            SaveCommand = new RelayCommand<BuildingViewModel>(x => x = CurrentBuilding);
            SerializeComand = new RelayCommand<BuildingViewModel>(x => Buildings.Select((b, id) => b.ToBuilding(id)));
            AddToRequirementCommand = new RelayCommand<BuildingViewModel>(x => currentBuilding.Requirements.Add(x), 
                x => !currentBuilding.Requirements.Contains(x));
        }

    }
}
