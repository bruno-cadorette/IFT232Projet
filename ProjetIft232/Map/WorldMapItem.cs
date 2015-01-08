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
        public int VisionRange { get; set; }
        //Make VISION
        [DataMember]
        public IEnumerable<KeyValuePair<Position, WorldMapItem>> VisionInterest { get; set; }
        [DataMember]
        public int PlayerId { get; set; }
        [DataMember]
        public bool CanBeDeleted { get; protected set; }
        public abstract WorldMapItem InteractWith(WorldMapItem item, Land land);
        public WorldMapItem()
        {
            CanBeDeleted = false;
        }

        public void UpdateVision(Dictionary<Position, WorldMapItem> vision)
        {
            VisionInterest = vision;
        }

        public int ItemInSight()
        {
            return VisionInterest.Count();
        }
    }
}
