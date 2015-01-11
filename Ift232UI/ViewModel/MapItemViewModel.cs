using Core;
using Core.Map;
using Core.Military;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Ift232UI
{
    public class MapItemViewModel
    {
        public MapCellInfo Info { get; set; }
        public Brush Brush
        {
            get
            {
                if (Info.Item == null)
                {
                    return Brushes.Gray;
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
                    return Brushes.Black;
                }
            }
        }
        public MapItemViewModel(MapCellInfo info)
        {
            Info = info;
        }
    }
}
