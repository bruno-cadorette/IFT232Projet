using Core.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{
    [DataContract]
    public class LandscapeConfig
    {
        [DataMember]
        public string TileSet { get; private set; }
        [DataMember]
        public IEnumerable<Land> Lands { get; private set; }
        [DataMember]
        public int Witdh { get; private set; }
        [DataMember]
        public int Height { get; private set; }

        public LandscapeConfig (string tileSet, IEnumerable<Land> lands, int width, int height)
        {
            TileSet = tileSet;
            Lands = lands;
            Witdh = width;
            Height = height;
        }
    }
}
