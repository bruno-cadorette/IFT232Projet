using GameHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameBuilder
{
    public class LandscapeViewModel : BindableBase
    {
        public int ID { get; set; }
        public BitmapSource Tile { get; set; }

        public ImageBrush Brush
        {
            get
            {
                return new ImageBrush(Tile);
            }
        }
        public static LandscapeViewModel DefaultLandscape { get; set; }

        public LandscapeViewModel(int id, BitmapSource tile)
        {
            ID = id;
            Tile = tile;
        }
        public static bool operator ==(LandscapeViewModel a, LandscapeViewModel b)
        {
            return a.ID == b.ID;
        }
        public static bool operator !=(LandscapeViewModel a, LandscapeViewModel b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return ID.ToString();
        }
    }
}
