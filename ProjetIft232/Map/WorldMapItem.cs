using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Map
{
    [DataContract]
    public abstract class WorldMapItem
    {
        [DataMember]
        public int PlayerId { get; set; }
        [DataMember]
        public bool CanBeDeleted { get; protected set; }
        public abstract WorldMapItem InteractWith(WorldMapItem item, Land land);
        public WorldMapItem()
        {
            CanBeDeleted = false;
        }
    }
}
