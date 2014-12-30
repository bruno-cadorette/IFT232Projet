using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GameBuilder
{
    public class LandscapeViewModel : BindableBase
    {
        private Color color;
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                SetProperty(ref color, value);
            }
        }
        public SolidColorBrush Brush
        {
            get
            {
                return new SolidColorBrush(Color);
            }
        }
        public LandscapeViewModel(Color color)
        {
            this.color = color;
        }
        public static bool operator ==(LandscapeViewModel a, LandscapeViewModel b)
        {
            return a.Color == b.Color;
        }
        public static bool operator !=(LandscapeViewModel a, LandscapeViewModel b)
        {
            return !(a == b);
        }
        public static LandscapeViewModel DefaultLandscape()
        {
            return new LandscapeViewModel(Brushes.Gray.Color);
        }
        public override string ToString()
        {
            return Color.ToString();
        }
    }
}
