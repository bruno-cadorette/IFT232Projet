using Core;
using Core.Military;
using GameHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ift232UI
{
    public class ArmyManagementViewModel : BindableBase
    {
        private City city;
        public IEnumerable<Soldier> SoldierTypes
        {
            get
            {
                return ArmyFactory.GetInstance().Soldiers();
            }
        }

        public IEnumerable<Groupment> CurrentArmy
        {
            get
            {
                return city.Army;
            }
        }

        public int NumberToCreate { get; set; }

        public ICommand TrainSoldiers { get; private set; }


        public ArmyManagementViewModel(City city)
        {
            this.city = city;
            TrainSoldiers = new RelayCommand<Soldier>(x => 
                {
                    city.AddArmy(x.ID, NumberToCreate);
                    OnPropertyChanged("Resources");
                }, x => x != null && NumberToCreate > 0);
        }




    }
}
