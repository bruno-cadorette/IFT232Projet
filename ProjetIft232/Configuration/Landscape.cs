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
    public class Landscape
    {
        [DataMember]
        public string TileSet { get; private set; }
        [DataMember]
        public IEnumerable<Land> Lands { get; private set; }

        public Landscape (string tileSet, IEnumerable<Land> lands)
        {
            TileSet = tileSet;
            Lands = lands;
        }
    }
}
