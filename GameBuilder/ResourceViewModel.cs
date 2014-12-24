using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBuilder
{
    class ResourceViewModel : BindableBase
    {
        private Resources resources;
        public ResourceViewModel() : this(new Resources())
        {
        }
        public ResourceViewModel(Resources resources)
        {
            this.resources = resources;
        }
        public int Gold
        {
            get
            { 
                return resources.Gold;
            }
            set
            {
                int property = resources.Gold;
                SetProperty(ref property, value);
                resources.Gold = property;
            }
        }
        public int Meat
        {
            get
            {
                return resources.Meat;
            }
            set
            {
                int property = resources.Meat;
                SetProperty(ref property, value);
                resources.Meat = property;
            }
        }
        public int Wood
        {
            get
            {
                return resources.Wood;
            }
            set
            {
                int property = resources.Wood;
                SetProperty(ref property, value);
                resources.Wood = property;
            }
        }
        public int Rock
        {
            get
            {
                return resources.Rock;
            }
            set
            {
                int property = resources.Rock;
                SetProperty(ref property, value);
                resources.Rock = property;
            }
        }
        public int Population
        {
            get
            {
                return resources.Population;
            }
            set
            {
                int property = resources.Population;
                SetProperty(ref property, value);
                resources.Population = property;
            }
        }
    }
}
