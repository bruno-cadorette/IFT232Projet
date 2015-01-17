using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Map
{
    public class MapCellInfo
    {
        public Land Land { get; set; }
        public WorldMapItem Item { get; set; }
        public bool IsVisible { get; set; }
    }
}
