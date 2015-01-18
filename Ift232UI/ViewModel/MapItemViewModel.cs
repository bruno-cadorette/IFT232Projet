using Core;
using Core.Map;
using Core.Military;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ift232UI
{
    public class MapItemViewModel
    {
        public MapCellInfo Info { get; private set; }
        private BitmapSource tile;
        public Brush Brush
        {
            get
            {
                if(!Info.IsVisible)
                {
                    return Brushes.Black;
                }
                if (Info.Item == null)
                {
                    return new ImageBrush(tile);
                }
                else if(Info.Item is City)
                {
                    return Brushes.Blue;
                }
                else if(Info.Item is Army)
                {
                    return Brushes.Red;
                }
                else
                {
                    return Brushes.Pink;
                }
            }
        }
        public MapItemViewModel(MapCellInfo info, BitmapSource tile)
        {
            Info = info;
            this.tile = tile;
        }
    }
}
